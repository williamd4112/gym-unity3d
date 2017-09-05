using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    public class HirerachicalRewardProducer : GameState
    {
        [SerializeField]
        private RewardProducer[] m_RewardProducers;

        public override byte[] GetState()
        {
            float reward = 0.0f;
            foreach(RewardProducer p in m_RewardProducers)
            {
                float r = p.GetReward();
                if (r != 0.0) {
                    reward = r;
                    break;
                }
            }
            return BitConverter.GetBytes(reward);
        }

        public override int GetStateSize()
        {
            return sizeof(float);
        }

        public override void Reset()
        {
            foreach(RewardProducer p in m_RewardProducers)
            {
                p.Reset();
            }
        }
    }

}

