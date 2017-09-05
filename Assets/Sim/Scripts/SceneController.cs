using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : SocketRawDataListener {

    /* Scene controller uses 0x00 ~ 0xff */
    private const int OP_RESET = 0xff;

    public override void OnReceiveRawData(ref byte[] data)
    {
        int opcode = BitConverter.ToInt32(data, 0);
        switch (opcode)
        {
            case OP_RESET:
                Debug.Log("Reset scene");
                Scene scene = SceneManager.GetActiveScene();
                GC.Collect();
                SceneManager.LoadScene(scene.name);
                break;
            default:
                break;
        }
    }

}
