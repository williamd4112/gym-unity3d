using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteIntegerDecoder : NetworkInputDecoder {

    private int m_Code;

    public int Code { get { return m_Code; } }

    public override void Decode(ref byte[] data)
    {
        int recvData = BitConverter.ToInt32(data, 0);

        m_Code = recvData;
    }
}
