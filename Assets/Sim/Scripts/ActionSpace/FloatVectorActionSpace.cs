using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    [System.Serializable]
    public class FloatVectorActionSpace : ActionSpace
    {
        public int m_NumElements = 2;

        public override int GetActionSpaceSize()
        {
            return sizeof(float) * m_NumElements;
        }
    }

}