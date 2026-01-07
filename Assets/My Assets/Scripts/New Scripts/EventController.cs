using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
public class EventController : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction playerMoveAction;
    private Vector2 playerMoveAmt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }
    private void Awake()
    {
        playerMoveAction = InputSystem.actions.FindAction("Move");
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerMoveAmt = playerMoveAction.ReadValue<Vector2>();
        
        EventManager.instance.OnJoystickMoveCaller(playerMoveAmt);
        
    }
}
