using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;

    [Header("Wheel Transforms")]
    public Transform frontRightWheelTransform;  
    public Transform frontLeftWheelTransform;
    public Transform rearRightWheelTransform;
    public Transform rearLeftWheelTransform;

    [Header("Joystick")]
    public FloatingJoystick joystick;  // Reference to your floating joystick

    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;  // Maximum torque the motor can apply
    public float maxSteerAngle = 30f;     // Maximum steering angle in degrees
    public float maxSpeed = 30f;           // Max speed in m/s (adjust as needed)

    [Header("Rigidbody and Center of Mass")]
    public Rigidbody carRigidbody;              // Assign your car's Rigidbody here
    public Transform centerOfMassTransform;    // Assign an empty GameObject for center of mass

    void Start()
    {
        if (carRigidbody != null && centerOfMassTransform != null)
        {
            // Set center of mass to improve car stability
            carRigidbody.centerOfMass = carRigidbody.transform.InverseTransformPoint(centerOfMassTransform.position);
        }
        carRigidbody.linearVelocity = transform.forward * 2f;

    }

    void FixedUpdate()
    {
        // Read joystick input and clamp between -1 and 1
        float verticalInput = Mathf.Clamp(joystick.Vertical, -1f, 1f);
        float horizontalInput = Mathf.Clamp(joystick.Horizontal, -1f, 1f);

        // Deadzone to prevent unwanted small inputs
        float deadzone = 0.1f;
        if (Mathf.Abs(verticalInput) < deadzone) verticalInput = 0f;
        if (Mathf.Abs(horizontalInput) < deadzone) horizontalInput = 0f;

        // Calculate current forward speed (m/s)
        float currentSpeed = Vector3.Dot(carRigidbody.linearVelocity, transform.forward);

        float motor = 0f;

        // Apply motor torque based on input and speed limits
        if (verticalInput > 0) // Forward
        {
            motor = (currentSpeed < maxSpeed) ? verticalInput * maxMotorTorque : verticalInput * maxMotorTorque * 0.1f;
        }
        else if (verticalInput < 0) // Reverse
        {
            motor = (currentSpeed > -maxSpeed) ? verticalInput * maxMotorTorque : verticalInput * maxMotorTorque * 0.1f;
        }
        else
        {
            motor = 0f;
        }

        float steering = horizontalInput * maxSteerAngle;

        ApplyMotorTorque(motor);
        ApplySteering(steering);
        UpdateWheels();
    }

    void ApplyMotorTorque(float motor)
    {
        frontLeftWheelCollider.motorTorque = motor;
        frontRightWheelCollider.motorTorque = motor;
        rearLeftWheelCollider.motorTorque = motor;
        rearRightWheelCollider.motorTorque = motor;
    }

    void ApplySteering(float steering)
    {
        frontLeftWheelCollider.steerAngle = steering;
        frontRightWheelCollider.steerAngle = steering;
    }

    void UpdateWheels()
    {
        UpdateWheelPose(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPose(rearRightWheelCollider, rearRightWheelTransform);
        UpdateWheelPose(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    void UpdateWheelPose(WheelCollider collider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
