using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public class SceneTerminationState : GameState
    {
        const string TARGET_TAG = "Target";

        [SerializeField]
        private CollisionEventTrigger[] m_ListenTargets;

        [SerializeField]
        private bool m_Done = false;

        void Start()
        {
            foreach (CollisionEventTrigger e in m_ListenTargets)
                e.OnTriggerEnterEvent += OnTriggerEnter;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(TARGET_TAG))
            {
                Debug.Log(collider.tag);
                m_Done = true;
            }
        }

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
    }

}