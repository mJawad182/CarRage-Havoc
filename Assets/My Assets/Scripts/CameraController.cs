using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 10f, -10f);  // Above and behind the car
    [Range(0.01f, 1f)] public float positionSmoothSpeed = 0.1f;
    [Range(0.01f, 1f)] public float rotationSmoothSpeed = 0.1f;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate desired position: offset relative to car's rotation
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Smoothly move camera to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);

        // Calculate desired rotation: look in the car's forward direction from camera position
        Vector3 lookDirection = target.forward;  // Car's forward vector

        // Option 1: Camera looks forward horizontally (ignore vertical difference)
        Vector3 lookPoint = target.position + lookDirection;
        lookPoint.y = transform.position.y;  // Keep camera looking horizontally forward

        Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        Vector3 euler = desiredRotation.eulerAngles;
        euler.x = 90f;  // Looking straight down
        euler.z = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), rotationSmoothSpeed);

    }
}
