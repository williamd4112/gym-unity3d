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
            Debug.Log("Reset scene");
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void Update()
    {
    }

}
