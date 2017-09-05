using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GymUnity3D
{
    abstract public class RewardProducer : GameState
    {
        public abstract float GetReward();
    }

}