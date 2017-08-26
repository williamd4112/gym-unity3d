using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kobuki
{
    [RequireComponent(typeof(VehicleController))]
    public class VehicleNetworkAdapter : FloatVectorDataListener
    {
        private VehicleController m_VehicleController;

        void Start()
        {
            m_VehicleController = GetComponent<VehicleController>();
        }

        public override void OnReceiveFloatVector(ref float[] data)
        {
            if (data.Length >= 2) {
                m_VehicleController.SetMotor(data[0] * m_VehicleController.GetMaxTorque());
                m_VehicleController.SetSteering(data[1] * m_VehicleController.GetMaxTorque());
            }
        }
    }
}
