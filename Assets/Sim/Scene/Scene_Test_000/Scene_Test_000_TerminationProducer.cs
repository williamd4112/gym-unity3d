using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Test_000_TerminationProducer : TerminationProducer {

    [SerializeField]
    private bool m_Done = false;

    public override byte[] GetTermination()
    {
        return BitConverter.GetBytes(m_Done);
    }

    public override int GetTerminationSize()
    {
        return sizeof(bool);
    }
	
}
