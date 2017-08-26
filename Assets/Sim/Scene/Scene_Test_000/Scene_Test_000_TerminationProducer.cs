using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Test_000_TerminationProducer : GymUnity3D.GameState {

    [SerializeField]
    private bool m_Done = false;

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
