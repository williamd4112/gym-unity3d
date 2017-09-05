using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public class VelocityRewardProducer : RewardProducer
    {
        [SerializeField]
        private Rigidbody m_Ref;

        [SerializeField]
        private float m_MaxValue = 5.0f;

        private float m_Reward = 0.0f;

        public override float GetReward()
        {
            Vector3 velocity = m_Ref.velocity;
            float z = velocity.z < m_MaxValue ? velocity.z : m_MaxValue;
            float x = velocity.x < m_MaxValue ? velocity.x : m_MaxValue;
            m_Reward = (z > 0.0f) ? z : 0.0f; 
            return m_Reward / m_MaxValue;
        }

        public override byte[] GetState()
        {
            float r = GetReward();
            return BitConverter.GetBytes(r);
        }

        public override int GetStateSize()
        {
            return sizeof(float);
        }

        public override void Reset()
        {
            m_Reward = 0.0f;
        }
    }

}