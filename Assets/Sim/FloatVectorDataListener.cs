using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class FloatVectorDataListener : SocketRawDataListener {

    public override void OnReceiveRawData(ref byte[] data)
    {
        /* Skip Scene Opcode */
        float[] v = Common.ConvertByteToFloat(data, sizeof(Int32));

        StringBuilder sb = new StringBuilder();
        foreach(float f in v)
        {
            sb.Append(f + ",");
        }
        Debug.Log(sb.ToString());
    }

}
