using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Common {

    public static float[] ConvertByteToFloat(byte[] array, int startIndex)
    {
        float[] floatArr = new float[(array.Length - startIndex) / sizeof(float)];
        for (int i = 0, j = startIndex; i < floatArr.Length && j < array.Length; i++, j += sizeof(float))
        {
            floatArr[i] = BitConverter.ToSingle(array, j);
        }
        return floatArr;
    }

    public static bool IsConnected(Socket socket)
    {
        try
        {
            return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
        }
        catch (SocketException) { return false; }
    }
}
