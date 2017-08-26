using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Test_000_RewardProducer : GymUnity3D.GameState{

    [SerializeField]
    private float m_Reward = 1.0f;

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
}
