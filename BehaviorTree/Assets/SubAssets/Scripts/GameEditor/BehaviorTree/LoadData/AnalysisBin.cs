using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnalysisBin
{
    public static void AnalysisData(byte[] byteData, Action<byte[]> CallBack)
    {
        ByteBufferRead bbr = new ByteBufferRead(byteData);
        int fileCount = bbr.ReadInt32();

        for (int i = 0; i < fileCount; ++i)
        {
            int start = bbr.ReadInt32();
            int length = bbr.ReadInt32();

            byte[] fileByteData = new byte[length];
            Array.Copy(byteData, start, fileByteData, 0, length);

            if (null != CallBack)
            {
                CallBack(fileByteData);
            }
        }
    }
}
