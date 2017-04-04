using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringDataListener : SocketRawDataListener {

    public override void OnReceiveRawData(ref byte[] data)
    {
        string res = System.Text.Encoding.ASCII.GetString(data);
        Debug.Log("Receieved: " + res);
    }
}
