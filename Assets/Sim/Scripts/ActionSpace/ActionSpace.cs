using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    [System.Serializable]
    abstract public class ActionSpace : MonoBehaviour
    {
        abstract public int GetActionSpaceSize();
    }
}