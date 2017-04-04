using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatVectorDataListener : SocketRawDataListener {

    public override void OnReceiveRawData(ref byte[] data)
    {
        float[] v = Common.ConvertByteToFloat(data);
        foreach(float v_ in v)
        {
            Debug.Log(v_);
        }
    }

}
