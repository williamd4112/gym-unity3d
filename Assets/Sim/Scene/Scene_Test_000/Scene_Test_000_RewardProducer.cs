using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Test_000_RewardProducer : RewardProducer {

    [SerializeField]
    private float m_Reward = 1.0f;

    public override byte[] GetReward()
    {
        return BitConverter.GetBytes(m_Reward);
    }

    public override int GetRewardSize()
    {
        return sizeof(float);
    }
}
