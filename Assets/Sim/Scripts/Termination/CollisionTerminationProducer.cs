using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public class CollisionTerminationProducer : GameState
    {
        [SerializeField]
        private bool m_Done = false;

        [SerializeField]
        private string m_TargetTag = "Target";

        [SerializeField]
        private CollisionEventTrigger m_CollisionSource;

        public override byte[] GetState()
        {
            return BitConverter.GetBytes(m_Done);
        }

        public override int GetStateSize()
        {
            return sizeof(bool);
        }

        public override void Reset()
        {
            m_Done = false;
        }

        // Use this for initialization
        void Start()
        {
            m_CollisionSource.OnCollisionStayEvent += OnCollisionStay;
        }

        void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag(m_TargetTag))
            {
                m_Done = true;
            }
        }
    }

}