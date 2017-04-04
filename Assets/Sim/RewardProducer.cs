using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RewardProducer : MonoBehaviour {
    abstract public byte[] GetReward();
    abstract public int GetRewardSize();
}
