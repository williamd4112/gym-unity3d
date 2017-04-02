using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraController : MonoBehaviour {

    [SerializeField]
    private DiscreteIntegerDecoder m_Decoder;

    [SerializeField]
    private float m_MoveSpeed = 10.0f;

	void Start () {
        Debug.Assert(m_Decoder != null);
	}
	
	void Update () {
        int action = m_Decoder.Code;
       
        Debug.Log("Decoder : " + action);
        switch (action)
        {
            case 1:
                transform.Translate(0f, 0f, Time.smoothDeltaTime * m_MoveSpeed);
                break;
            case 2:
                transform.Translate(0f, 0f, Time.smoothDeltaTime * m_MoveSpeed * -1.0f);
                break;
            default:
                break;
        }
    }
}
