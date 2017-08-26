using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GymUnity3D;

namespace Capture_Straight
{
    public class SceneRewardState : GameState
    {
        const float REWARD_FOR_TARGET = 1.0f;
        const string TARGET_TAG = "Target";

        private float m_Reward = 0;

        [SerializeField]
        private CollisionEventTrigger m_ListenTarget;

        void Start()
        {
            m_ListenTarget.OnTriggerEnterEvent += OnTriggerEnter;
            m_ListenTarget.OnTriggerStayEvent += OnTriggerStay;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(TARGET_TAG))
            {
                m_Reward = REWARD_FOR_TARGET;
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.CompareTag(TARGET_TAG))
            {
                m_Reward = REWARD_FOR_TARGET;
            }
        }

        public override byte[] GetState()
        {
            return BitConverter.GetBytes(m_Reward);
        }

        public override int GetStateSize()
        {
            return sizeof(float);
        }

        public override void Reset()
        {
            m_Reward = 0;
        }
    }
}

