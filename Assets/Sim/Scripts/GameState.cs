using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    abstract public class GameState : MonoBehaviour
    {
        abstract public byte[] GetState();
        abstract public int GetStateSize();
        abstract public void Reset();
    }
}
