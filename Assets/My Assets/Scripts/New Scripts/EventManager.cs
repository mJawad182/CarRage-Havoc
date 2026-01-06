using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public event Action<Vector2> OnJoystickMove;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    public void OnJoystickMoveCaller(Vector2 value)
    {
        OnJoystickMove?.Invoke(value);
    }
}
