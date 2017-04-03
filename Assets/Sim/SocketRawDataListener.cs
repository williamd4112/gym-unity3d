using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SocketRawDataListener : MonoBehaviour {
    abstract public void OnReceiveRawData(ref byte[] data);
}
