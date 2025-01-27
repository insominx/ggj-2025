using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using XRInputDevice = UnityEngine.XR.InputDevice;

public enum StylusHandSide
{
    Left,
    Right,
    Unknown
}

public class StylusEventRelay : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    InputActionAsset inputActionsAsset; // Input Action asset containing all stylus actions

    [SerializeField]
    GameObject leftHandStylus;

    [SerializeField]
    GameObject rightHandStylus;

    [Header("Stylus Events")]
    public FloatEvent onTipActionPerformed;
    public BoolEvent onGrabActionPerformed;
    public BoolEvent onGrabActionCanceled;
    public BoolEvent onOptionActionPerformed;
    public FloatEvent onMiddleActionPerformed;

    [Header("Stylus Connection Events")]
    public StylusConnectionEvent onStylusConnected;
    public StylusConnectionEvent onStylusDisconnected;

    InputAction tipAction;
    InputAction grabAction;
    InputAction optionAction;
    InputAction middleAction;

    XRInputDevice stylus;

    //---------------------------------------------------------------------------
    void Awake()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
        InputDevices.deviceDisconnected += OnDeviceDisconnected;

        if (inputActionsAsset != null)
        {
            var actionMap = inputActionsAsset.FindActionMap("MX_Ink"); // Replace with your action map name
            if (actionMap != null)
            {
                tipAction = actionMap.FindAction("Ink_Tip");
                middleAction = actionMap.FindAction("Ink_MiddleButton");
                grabAction = actionMap.FindAction("Grab");
                optionAction = actionMap.FindAction("Option");

                // actions have to be turned on or they do nothing
                tipAction.Enable();
                grabAction.Enable();
                optionAction.Enable();
                middleAction.Enable();

                tipAction.performed += OnTipActionPerformed;
                grabAction.performed += OnGrabActionPerformed;
                grabAction.canceled += OnGrabActionCanceled;
                optionAction.performed += OnOptionActionPerformed;
                middleAction.performed += OnMiddleActionPerformed;
            }
            else
            {
                Debug.LogError("Stylus action map not found in the InputActionAsset.");
            }
        }
        else
        {
            Debug.LogError("InputActionAsset is not assigned. Please assign it in the inspector.");
        }

        var devices = new List<XRInputDevice>();
        InputDevices.GetDevices(devices);

        // Initial device connection check
        foreach (var device in devices)
            OnDeviceConnected(device);
    }

    //---------------------------------------------------------------------------
    void OnGrabActionCanceled(InputAction.CallbackContext _)
    {
        onGrabActionCanceled.Invoke(false);
    }

    //---------------------------------------------------------------------------
    void OnDestroy()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
        InputDevices.deviceDisconnected -= OnDeviceDisconnected;

        if (tipAction != null) tipAction.performed -= OnTipActionPerformed;
        if (grabAction != null) grabAction.performed -= OnGrabActionPerformed;
        if (optionAction != null) optionAction.performed -= OnOptionActionPerformed;
        if (middleAction != null) middleAction.performed -= OnMiddleActionPerformed;
    }

    //---------------------------------------------------------------------------
    void OnDeviceConnected(XRInputDevice device)
    {
        if (device.name.ToLower().Contains("logitech"))
        {
            bool isRight = (device.characteristics & InputDeviceCharacteristics.Right) != 0;
            stylus = device;
            var hand = isRight ? StylusHandSide.Right : StylusHandSide.Left;
            Debug.Log($"Stylus connected on {hand} hand.");
            onStylusConnected.Invoke(hand);
        }
    }

    //---------------------------------------------------------------------------
    void OnDeviceDisconnected(XRInputDevice device)
    {
        if (device.name.ToLower().Contains("logitech") && stylus.Equals(device))
        {
            var isRight = (device.characteristics & InputDeviceCharacteristics.Right) != 0;
            var hand = isRight ? StylusHandSide.Right : StylusHandSide.Left;
            stylus = default;
            Debug.Log($"Stylus disconnected from {hand} hand.");
            onStylusDisconnected.Invoke(hand);
        }
    }

    //---------------------------------------------------------------------------
    void OnTipActionPerformed(InputAction.CallbackContext context)
    {
        float tipValue = context.ReadValue<float>();
        onTipActionPerformed.Invoke(tipValue);
    }

    //---------------------------------------------------------------------------
    void OnGrabActionPerformed(InputAction.CallbackContext context)
    {
        // bool isGrabPressed = context.ReadValueAsButton();
        onGrabActionPerformed.Invoke(true);
    }

    //---------------------------------------------------------------------------
    void OnOptionActionPerformed(InputAction.CallbackContext context)
    {
        bool isOptionPressed = context.ReadValueAsButton();
        onOptionActionPerformed.Invoke(isOptionPressed);
    }

    //---------------------------------------------------------------------------
    void OnMiddleActionPerformed(InputAction.CallbackContext context)
    {
        float middleValue = context.ReadValue<float>();
        onMiddleActionPerformed.Invoke(middleValue);
    }
}
