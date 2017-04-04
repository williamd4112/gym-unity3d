using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneResetHandler : SocketRawDataListener {

    private const int RESET_INT = 0xff;

    [SerializeField]
    private bool m_IsReset = false;

    public override void OnReceiveRawData(ref byte[] data)
    {
        if (BitConverter.ToInt32(data, 0) == RESET_INT)
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
