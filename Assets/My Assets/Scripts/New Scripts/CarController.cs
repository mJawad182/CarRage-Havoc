using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Car Movement Components")]
    public float maxSpeed = 5;
    

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

    [Header("Engine Machanics")]
    public float maxMotorTorque = 1500f;
    public float maxStearingAngle = 30;
    public float brakeForce = 3000;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.instance.OnJoystickMove += CarMovement;

        rb = GetComponent<Rigidbody>();
    }

    private void CarMovement(Vector2 obj)
    {
        //Vector3 movement = new Vector3(obj.x, 0, obj.y);
        //rb.MovePosition(rb.position+movement * speed * Time.deltaTime);

        float verticalInput = Mathf.Clamp(obj.y, -1,1);
        float horizontalInput = Mathf.Clamp(obj.x, -1,1);

        float motor = 0f;
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if(verticalInput > 0)
        {
            motor = (currentSpeed < maxSpeed) ? verticalInput * maxMotorTorque : verticalInput * maxMotorTorque * 0.1f;
        }else if(verticalInput< 0)
        {
            motor = (currentSpeed > -maxSpeed) ? verticalInput * maxMotorTorque : verticalInput * maxMotorTorque * 0.1f;
        }
        else
        {
            motor = 0;
        }

        ApplyMotorTorque(motor);
        float stearingAngle = horizontalInput * maxStearingAngle;
        ApplyStearingAngle(stearingAngle);

        if (Mathf.Abs(verticalInput) < 0.05f)
        {
            ApplyBrakeTorque(brakeForce);
        }
        else
            ApplyBrakeTorque(0f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyMotorTorque(float motor)
    {
        frontLeftWheelCollider.motorTorque = motor;
        frontRightWheelCollider.motorTorque = motor;
        rearLeftWheelCollider.motorTorque = motor;
        rearRightWheelCollider.motorTorque = motor;
    }
    private void ApplyStearingAngle(float stearing)
    {
        frontLeftWheelCollider.steerAngle = stearing;
        frontRightWheelCollider.steerAngle = stearing;
    }
    private void ApplyBrakeTorque(float brake)
    {
        frontLeftWheelCollider.brakeTorque = brake;
        frontRightWheelCollider.brakeTorque = brake;
        rearLeftWheelCollider.brakeTorque = brake;
        rearRightWheelCollider.brakeTorque = brake;
    }
}
