using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class FloatVectorDataListener : SocketRawDataListener {

    const int OP_FLOAT_VEC = 0x01ff;

    sealed public override void OnReceiveRawData(ref byte[] data)
    {
        int opcode = BitConverter.ToInt32(data, 0);
        if (opcode == OP_FLOAT_VEC)
        {
            /* Skip Scene Opcode */
            float[] v = Common.ConvertByteToFloat(data, sizeof(Int32));

            OnReceiveFloatVector(ref v);
        }
    }

    virtual public void OnReceiveFloatVector(ref float[] data)
    {
        StringBuilder sb = new StringBuilder();
        foreach (float f in data)
        {
            sb.Append(f + ",");
        }
        Debug.Log(sb.ToString());
    }

}
