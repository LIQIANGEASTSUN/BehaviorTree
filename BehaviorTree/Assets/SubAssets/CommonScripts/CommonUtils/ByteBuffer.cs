using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class ByteBufferWrite
{
    private List<byte> byteList = new List<byte>();

    private bool useFixedLength = false;
    private int writeIndex = 0;
    private byte[] byteData = null;

    private Dictionary<int, byte[]> _byteCacheDic = new Dictionary<int, byte[]>() {
        {1, new byte[1] },
        {2, new byte[2] },
        {4, new byte[4] },
        {8, new byte[8] }
    };
    public ByteBufferWrite()
    {
        byteList = new List<byte>();
    }

    public ByteBufferWrite(int capacity)
    {
        byteData = new byte[capacity];
        useFixedLength = true;
        writeIndex = 0;
    }

    #region Write

    private byte[] byteArr = new byte[1];
    public void WriteByte(byte value)
    {
        byteArr[0] = value;
        WriteBytes(byteArr, byteArr.Length);
    }

    public void WriteSByte(sbyte value)
    {
        WriteByte((byte)value);
    }

    public void WriteChar(char value)
    {
        byte[] data = _byteCacheDic[2];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteShort(short value)
    {
        byte[] data = _byteCacheDic[2];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteUShort(ushort value)
    {
        byte[] data = _byteCacheDic[2];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteUInt16(UInt16 value)
    {
        byte[] data = _byteCacheDic[2];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteInt16(Int16 value)
    {
        byte[] data = _byteCacheDic[2];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteUInt32(UInt32 value)
    {
        byte[] data = _byteCacheDic[4];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteInt32(Int32 value)
    {
        byte[] data = _byteCacheDic[4];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteUInt64(UInt64 value)
    {
        byte[] data = _byteCacheDic[8];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteInt64(Int64 value)
    {
        byte[] data = _byteCacheDic[8];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public unsafe void WriteSingle(float value)
    {
        byte[] data = _byteCacheDic[4];
        Bit.BitConverter.GetBytes(value, ref data);
        WriteBytes(data, data.Length);
    }

    public void WriteVector2(Vector2 vector)
    {
        WriteSingle(vector.x);
        WriteSingle(vector.y);
    }

    public void WriteVector3Int(Vector3Int vector3Int)
    {
        WriteInt32(vector3Int.x);
        WriteInt32(vector3Int.y);
        WriteInt32(vector3Int.z);
    }

    public void WriteVector3(Vector3 vector)
    {
        WriteSingle(vector.x);
        WriteSingle(vector.y);
        WriteSingle(vector.z);
    }

    public void WriteString(string str)
    {
        byte[] byteData = Encoding.UTF8.GetBytes(str);
        WriteInt32(byteData.Length);
        WriteBytes(byteData, byteData.Length);
    }

    private const string exception = "RangeOut";
    public void WriteBytes(byte[] data, int length)
    {
        if (useFixedLength)
        {
            bool result = (null != byteData) && ((writeIndex + data.Length) <= byteData.Length);
            Assert.IsTrue(result, exception);
            if (result)
            {
                Array.Copy(data, 0, byteData, writeIndex, data.Length);
                writeIndex += data.Length;
            }
        }
        else
        {
            for (int i = 0; i < data.Length; ++i)
            {
                byteList.Add(data[i]);
            }
        }
    }
    #endregion

    public byte[] GetBytes()
    {
        if (useFixedLength)
        {
            return byteData;
        }
        else
        {
            return byteList.ToArray<byte>();
        }
    }

    public void Clear()
    {
        if (useFixedLength)
        {
            writeIndex = 0;
        }
        else
        {
            byteList.Clear();
        }
    }

    public void SetUseFixedLength(bool value)
    {
        useFixedLength = value;
    }

    public void Seek(int index)
    {
        writeIndex = index;
    }

}

public class ByteBufferRead
{
    private byte[] byteData = null;
    private int readIndex = 0;

    public ByteBufferRead()
    {

    }

    public ByteBufferRead(byte[] data)
    {
        byteData = new byte[data.Length];
        Array.Copy(data, 0, byteData, 0, data.Length);
    }

    public byte[] GetBytes()
    {
        return byteData;
    }

    public void ResetData(byte[] data)
    {
        byteData = data;
        Seek(0);
    }

    #region Read
    public byte ReadByte()
    {
        byte value = default(byte);
        int length = sizeof(byte);
        if (IsOutRange(readIndex, (length)))
        {
            Debug.LogError("Oot Range:" + readIndex);
            return value;
        }

        value = byteData[readIndex];

        ReadCount(length);
        return value;
    }

    public sbyte ReadSByte()
    {
        sbyte value = default(sbyte);
        int length = sizeof(sbyte);
        if (IsOutRange(readIndex, length))
        {
            Debug.LogError("Out Range:" + readIndex);
            return value;
        }

        value = (sbyte)byteData[readIndex];

        ReadCount(length);
        return value;
    }

    public char ReadChar()
    {
        char value = char.MinValue;
        int length = sizeof(char);
        if (IsOutRange(readIndex, length))
        {
            return value;
        }

        value = Bit.BitConverter.ToChar(byteData, readIndex);// BitConverter.ToChar(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public short ReadShort()
    {
        short value = 0;
        int length = sizeof(short);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToInt16(byteData, readIndex);// BitConverter.ToInt16(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public UInt16 ReadUInt16()
    {
        UInt16 value = 0;
        int length = sizeof(UInt16);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToUInt16(byteData, readIndex);// BitConverter.ToUInt16(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public Int16 ReadInt16()
    {
        Int16 value = 0;
        int length = sizeof(Int16);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToInt16(byteData, readIndex);// BitConverter.ToInt16(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public UInt32 ReadUInt32()
    {
        UInt32 value = 0;
        int length = sizeof(UInt32);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToUInt32(byteData, readIndex);// BitConverter.ToUInt32(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public Int32 ReadInt32()
    {
        Int32 value = 0;
        int length = sizeof(Int32);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToInt32(byteData, readIndex);// BitConverter.ToInt32(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public UInt64 ReadUInt64()
    {
        UInt64 value = 0;
        int length = sizeof(UInt64);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToUInt64(byteData, readIndex);// BitConverter.ToUInt64(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public Int64 ReadInt64()
    {
        Int64 value = 0;
        int length = sizeof(Int64);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToInt64(byteData, readIndex);// BitConverter.ToInt64(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public Single ReadSingle()
    {
        Single value = 0;
        int length = sizeof(Single);
        if (IsOutRange(readIndex, (length)))
        {
            return value;
        }

        value = Bit.BitConverter.ToSingle(byteData, readIndex);// BitConverter.ToSingle(byteData, readIndex);

        ReadCount(length);
        return value;
    }

    public Vector2 ReadVector2()
    {
        Single x = ReadSingle();
        Single y = ReadSingle();

        return new Vector2(x, y);
    }

    public Vector3Int ReadVector3Int()
    {
        Int32 x = ReadInt32();
        Int32 y = ReadInt32();
        Int32 z = ReadInt32();

        return new Vector3Int(x, y, z);
    }

    public Vector3 ReadVector3()
    {
        Single x = ReadSingle();
        Single y = ReadSingle();
        Single z = ReadSingle();

        return new Vector3(x, y, z);
    }

    public string ReadString(int length)
    {
        byte[] byteData = ReadBytes(length);
        string str = Encoding.UTF8.GetString(byteData);
        return str;
    }

    public byte[] ReadBytes(int length)
    {
        byte[] data = new byte[length];
        if (IsOutRange(readIndex, length))
        {
            return data;
        }

        Array.Copy(byteData, readIndex, data, 0, length);
        ReadCount(length);

        return data;
    }

    private void ReadCount(int count)
    {
        readIndex += count;
    }

    private int outRange = 0;
    private bool IsOutRange(int start, int length)
    {
        if ((start + length) > byteData.Length)
        {
            Debug.LogError("Out of Range Bytes: totalSize:" + byteData.Length + "    readSize:" + (start + length) + "     outRange:" + (++outRange));
            return true;
        }

        return false;
    }

    public void Seek(int pos)
    {
        pos = Mathf.Clamp(pos, 0, byteData.Length);
        readIndex = pos;
    }

    public int SeekIndex()
    {
        return readIndex;
    }

    public bool IsReadEnd() 
    {
        return readIndex >= byteData.Length;
    }

    public Int32 Length { get { return byteData.Length; } }

    public int Residue { get { return Length - readIndex; } }

    public void Clear()
    {
        byteData = null;
    }

    public void ReplaceBytes(byte[] byteArr, int pos)
    {
        ByteBufferReplace.ReplaceBytes(byteData, byteArr, pos);
    }

    public void ReplaceByte(byte value, int pos)
    {
        ByteBufferReplace.ReplaceByte(byteData, value, pos);
    }
    #endregion

}

public static class ByteBufferReplace
{
    public static byte[] ReplaceInt32(byte[] byteData, Int32 value, int pos)
    {
        byte[] data = BitConverter.GetBytes(value);

        byteData = ReplaceBytes(byteData, data, pos);

        return byteData;
    }

    public static byte[] ReplaceBytes(byte[] byteData, byte[] byteArr, int pos)
    {
        for (int i = 0; i < byteArr.Length; ++i)
        {
            byteData = ReplaceByte(byteData, byteArr[i], pos + i);
        }

        return byteData;
    }

    public static byte[] ReplaceByte(byte[] byteData, byte value, int pos)
    {
        if (RangeOut(byteData, pos))
        {
            Debug.LogError("Range Out");
            return byteData;
        }

        byteData[pos] = value;

        return byteData;
    }

    private static bool RangeOut(byte[] byteData, int pos)
    {
        return pos <0 || pos >= byteData.Length;
    }

}

