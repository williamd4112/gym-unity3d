using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    [Serializable]
    public struct CollisionRewardLookupEntry
    {
        [SerializeField]
        public string tag;
        [SerializeField]
        public float reward;
    }

    public class CollisionRewardProducer : RewardProducer
    {
        [SerializeField]
        private float m_Reward = 0.0f;

        [SerializeField]
        private CollisionRewardLookupEntry[] m_Entries;
        private Dictionary<string, float> m_CollisionRewardLookupTable = new Dictionary<string, float>();

        [SerializeField]
        private CollisionEventTrigger m_CollisionSource;

        // Use this for initialization
        void Start()
        {
            m_CollisionSource.OnCollisionStayEvent += OnCollisionStay;
            foreach (CollisionRewardLookupEntry e in m_Entries)
            {
                m_CollisionRewardLookupTable.Add(e.tag, e.reward);
            }
        }

        void OnCollisionStay(Collision collision)
        {
            string tag = collision.gameObject.tag;
            float reward = 0.0f;
            if (m_CollisionRewardLookupTable.TryGetValue(tag, out reward))
            {
                m_Reward += reward;
                if (m_Reward < -1.0f) m_Reward = -1.0f;
                if (m_Reward > 1.0f) m_Reward = 1.0f;
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
            m_Reward = 0.0f;
        }

        public override float GetReward()
        {
            return m_Reward;
        }
    }
}
