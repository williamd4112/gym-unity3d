using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class ObservationProducer : MonoBehaviour {

    abstract public void GetObservation(out byte[] buffer);
}
