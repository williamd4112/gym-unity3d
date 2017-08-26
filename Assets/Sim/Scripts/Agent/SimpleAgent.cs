using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public class SimpleAgent : MonoBehaviour
    {
        [SerializeField]
        private float h = 0.0f;
        [SerializeField]
        private float v = 0.0f;

        private Rigidbody m_Rigidbody;

        // Use this for initialization
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        void FixedUpdate()
        {
            m_Rigidbody.velocity = new Vector3(0, 0, v);
                        
        }
    }

}