using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleController : MonoBehaviour
{
    [SerializeField]
    private AxleInfo axleInfo; // the information about each individual axle
    [SerializeField]
    private float maxMotorTorque; // maximum torque the motor can apply to wheel

    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float leftMotor = motor;
        float rightMotor = motor;

        if (axleInfo.steering)
        {
            float steering = Input.GetAxis("Horizontal");

            if (steering > 0)
            {
                rightMotor = rightMotor * 0.2f + 0.8f * (1f - steering);
            }
            else if (steering < 0)
            {
                leftMotor = leftMotor * 0.2f + 0.8f * (1f - Mathf.Abs(steering));
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