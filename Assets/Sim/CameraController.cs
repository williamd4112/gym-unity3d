using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SocketRawDataListener {

    [SerializeField]
    private float m_Horizontal;
    [SerializeField]
    private float m_Vertical;
	Camera m_camera ;
    public override void OnReceiveRawData(ref byte[] data)
    {
        float[] velocity = Common.ConvertByteToFloat(data, 0);
        m_Horizontal = velocity[0];
        m_Vertical = velocity[1];
        Debug.Log(velocity[0] + "," + velocity[1]);
    }

    // Use this for initialization
    void Start () {
		m_camera = GetComponent< Camera> ();
	}
	void Update(){
		m_camera.Render ();
	}
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(new Vector3(m_Horizontal, 0.0f, m_Vertical) * Time.fixedDeltaTime);
	}
}
