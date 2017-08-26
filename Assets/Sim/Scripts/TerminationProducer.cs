using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TerminationProducer : MonoBehaviour {
    abstract public byte[] GetTermination();
    abstract public int GetTerminationSize();
}
