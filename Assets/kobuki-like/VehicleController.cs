using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Kobuki
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField]
        private AxleInfo axleInfo; // the information about each individual axle
        [SerializeField]
        private float maxMotorTorque; // maximum torque the motor can apply to wheel

        [SerializeField]
        private float m_Motor;
        [SerializeField]
        private float m_Steering;
        [SerializeField]
        private bool m_HumanControl = false;

        private int m_FrameCount = 0;

        [SerializeField]
        private int m_FrameSkip = 5;

        public void SetFrameCount(int c)
        {
            m_FrameCount = Time.frameCount;
        }

        public float GetMaxTorque()
        {
            return maxMotorTorque;
        }

        public void SetMotor(float motor)
        {
            m_Motor = motor;
        }

        public void SetSteering(float steer)
        {
            m_Steering = steer;
        }

        void Awake()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if ("skip".Equals(args[i]))
                {
                    m_FrameSkip = int.Parse(args[i + 1]);
                }
            }
        }

        void Update()
        {
            if (Time.frameCount - m_FrameCount > m_FrameSkip)
            {
                m_Motor = 0.0f;
                m_Steering = 0.0f;
            }

            if (m_HumanControl) {
                m_Motor = maxMotorTorque * Input.GetAxis("Vertical");
                m_Steering = Input.GetAxis("Horizontal");
            }
        }

        public void FixedUpdate()
        {
            float leftMotor = m_Motor;
            float rightMotor = m_Motor;

            if (axleInfo.steering)
            {
                if (m_Steering > 0)
                {
                    rightMotor = rightMotor * 0.2f + 0.8f * (1f - m_Steering);
                }
                else if (m_Steering < 0)
                {
                    leftMotor = leftMotor * 0.2f + 0.8f * (1f - Mathf.Abs(m_Steering));
                }
            }

            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = leftMotor;
                axleInfo.rightWheel.motorTorque = rightMotor;
            }
        }
    }

    [System.Serializable]
    public class AxleInfo
    {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }
}