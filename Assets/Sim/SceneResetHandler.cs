using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetHandler : SocketRawDataListener {

    private const byte RESET_BYTE = 0xff;

    [SerializeField]
    private bool m_IsReset = false;

    public override void OnReceiveRawData(ref byte[] data)
    {
        if (data[0] == RESET_BYTE)
        {
            m_IsReset = true;
        }
    }

    void Update()
    {
        if (m_IsReset)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

}
