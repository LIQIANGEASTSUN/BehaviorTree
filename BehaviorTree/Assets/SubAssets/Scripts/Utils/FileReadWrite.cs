using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileReadWrite  {

	public static bool Write(string path, string content)
    {
        byte[] data = System.Text.Encoding.Default.GetBytes(content);
        return Write(path, data);
    }

    public static bool Write(string path, byte[] byteData)
    {
        bool value = false;
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(byteData, 0, byteData.Length);
                fs.Dispose();
                fs.Close();
            }

            value = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return value;
    }

    public static string Read(string path)
    {
        string content = string.Empty;

        if (!File.Exists(path))
        {
            return content;
        }

        try
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                content = sr.ReadToEnd();
                sr.Dispose();
                sr.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return content;
    }

    public static byte[] ReadBytes(string path)
    {
        byte[] byteData = new byte[] { };

        if (!File.Exists(path))
        {
            return byteData;
        }

        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                long length = fs.Length;
                byteData = new byte[length];
                fs.Seek(0, SeekOrigin.Begin);
                fs.Read(byteData, 0, (int)length);
                fs.Dispose();
                fs.Close();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        return byteData;
    }
}
