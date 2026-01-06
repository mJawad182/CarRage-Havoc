using UnityEngine;

public class CarController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.instance.OnJoystickMove += CarMovement;
    }

    private void CarMovement(Vector2 obj)
    {
        Debug.Log(obj.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
