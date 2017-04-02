using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class NetworkInputDecoder : MonoBehaviour {
    abstract public void Decode(ref byte[] data);
}
