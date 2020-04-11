using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class TableRead
{
    public static readonly TableRead Instance = new TableRead();
    private Dictionary<string, TableData> _tableDic = new Dictionary<string, TableData>();

    public void Init()
    {
        _tableDic.Clear();
    }

    public void ReadCustomPath(string path)
    {
        DirectoryInfo dInfo = new DirectoryInfo(path);
        FileInfo[] fileInfoArr = dInfo.GetFiles("*.csv", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < fileInfoArr.Length; ++i)
        {
            string filePath = fileInfoArr[i].FullName;
            Read(filePath);
        }
    }

    private void Read(string filePath)
    {
        string content = FileReadWrite.Read(filePath);
       
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        TableData tableData = new TableData(fileName);
        tableData.SetData(content);
        _tableDic[fileName] = tableData;
    }

    public string GetData(string tableName, int key, string colName)
    {
        TableData tableData = null;
        if (!_tableDic.TryGetValue(tableName, out tableData))
        {
            return string.Empty;
        }

        return tableData.GetData(key, colName);
    }

    public List<int> GetKeyList(string tableName)
    {
        TableData tableData = null;
        if (!_tableDic.TryGetValue(tableName, out tableData))
        {
            return new List<int>();
        }

        return tableData.GetKeyList();
    }
    
}

public class TableData
{
    private string _fileName = string.Empty;
    private Dictionary<string, int> _colDic = new Dictionary<string, int>();
    private Dictionary<int, int> _rowDic = new Dictionary<int, int>();
    private List<List<string>> _contentList = new List<List<string>>();

    public TableData(string fileName)
    {
        _fileName = fileName;
    }

    public void SetData(string content)
    {
        _contentList.Clear();

        int offset = 0;
        int row = 0;
        bool hasKey = false;
        while (true)
        {
            List<string> lineList = readCsvLine(content, ref offset);
            if (lineList.Count <= 0)
            {
                break;
            }

            if (!hasKey)
            {
                hasKey = true;
                for (int i = 0; i < lineList.Count; ++i)
                {
                    _colDic[lineList[i]] = i;
                }
            }

            string keyS = lineList[0];
            if (string.IsNullOrEmpty(keyS))
            {
                continue;
            }

            int key = 0;
            if (!int.TryParse(keyS, out key))
            {
                continue;
            }

            _rowDic[key] = row;
            ++row;

            _contentList.Add(lineList);
        }
    }

    public string GetData(int key, string colName)
    {
        int row = 0;
        int col = 0;
        if (!_rowDic.TryGetValue(key, out row))
        {
            return string.Empty;
        }

        if (!_colDic.TryGetValue(colName, out col))
        {
            return string.Empty;
        }

        if (row >= _contentList.Count)
        {
            return string.Empty;
        }

        if (col >= _contentList[row].Count)
        {
            return string.Empty;
        }

        return _contentList[row][col];
    }

    public List<int> GetKeyList()
    {
        List<int> keyList = new List<int>();
        foreach(var kv in _rowDic)
        {
            keyList.Add(kv.Key);
        }
        return keyList;
    }

    public List<string> readCsvLine(string csvContent, ref int offset)
    {
        List<string> splitList = new List<string>();
        bool bInQuato = false;

        int len = csvContent.Length;
        string word = "";
        while (offset < len)
        {
            if (!bInQuato && csvContent[offset] == '"')
            {
                bInQuato = true;
                offset++;
                continue;
            }
            if (bInQuato && csvContent[offset] == '"')
            {
                if (csvContent[offset + 1] == '"')
                {
                    word += '"';
                    offset += 2;
                    continue;
                }
                bInQuato = false;
                offset++;
                continue;
            }

            if (!bInQuato)
            {
                if (csvContent[offset] == ',')
                {
                    splitList.Add(word);
                    offset++;
                    word = "";
                    continue;
                }
                if (csvContent[offset] == '\r')
                {
                    offset++;
                    continue;
                }
                if (csvContent[offset] == '\n')
                {
                    splitList.Add(word);
                    offset++;
                    return splitList;
                }
            }
            word += csvContent[offset];
            offset++;
        }
        return splitList;
    }

}


