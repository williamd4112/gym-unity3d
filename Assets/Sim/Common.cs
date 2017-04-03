using System.Collections;
using System.Collections.Generic;
using System;

public class Common {

    public static float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        for (int i = 0; i < floatArr.Length; i++)
        {
            floatArr[i] = BitConverter.ToSingle(array, i * 4);
        }
        return floatArr;
    }
}
