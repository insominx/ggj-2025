using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class LogitechStylusDemo : MonoBehaviour
{
    [Header("Configuration")]
    public bool demoPeriodicHaptic;

    [Header("References")]
    [SerializeField] InputActionAsset inputActionsAsset; // Input Action asset containing all stylus actions

    [Header("Feedback - Don't Edit")]
    public bool stylusOnRightHand;

    InputAction tipAction;
    InputAction grabAction;
    InputAction optionAction;
    InputAction middleAction;

    InputDevice stylus;

    void Awake()
    {
        if (inputActionsAsset == null)
        {
            Debug.LogError("InputActionAsset is not assigned. Please assign it in the inspector.");
            return;
        }

        tipAction = inputActionsAsset.FindAction("Ink_Tip", true);
        grabAction = inputActionsAsset.FindAction("Grab", true);
        optionAction = inputActionsAsset.FindAction("Option", true);
        middleAction = inputActionsAsset.FindAction("Ink_MiddleButton", true);

        // Input actions are disabled by default so we enable them
        tipAction.Enable();
        grabAction.Enable();
        optionAction.Enable();
        middleAction.Enable();

        tipAction.performed += OnTipActionPerformed;
        grabAction.performed += OnGrabActionPerformed;
        optionAction.performed += OnOptionActionPerformed;
        middleAction.performed += OnMiddleActionPerformed;

        InputDevices.deviceConnected += OnInputDeviceConnected;
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => stylus != null && demoPeriodicHaptic);
        Debug.Log("Beginning haptic test");

        while (true)
        {
            if (demoPeriodicHaptic)
            {
                stylus.SendHapticImpulse(0, 0.5f, 0.3f);
                yield return new WaitForSeconds(3f);
            }
        }
    }

    void Update()
    {

    }

    void OnDestroy()
    {
        // Unsubscribe from the action events
        tipAction.performed -= OnTipActionPerformed;
        grabAction.performed -= OnGrabActionPerformed;
        optionAction.performed -= OnOptionActionPerformed;
        middleAction.performed -= OnMiddleActionPerformed;
    }

    // This happens any time pressure is applied to the tip
    void OnTipActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"Tip Action Performed: Value = {context.ReadValue<float>()}");
    }

    // This refers to the button closest to the tip
    void OnGrabActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"Grab Action Performed: IsPressed = {context.ReadValueAsButton()}");
    }

    // This refers to the button farthest from the tip
    void OnOptionActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"Option Action Performed: IsPressed = {context.ReadValueAsButton()}");
    }

    // This refers to the analog, long rectangular button in the middle
    void OnMiddleActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log($"Middle Action Performed: Value = {context.ReadValue<float>()}");
    }

    void OnInputDeviceConnected(InputDevice newDevice)
    {
        bool mxInkConnected = newDevice.name.ToLower().Contains("logitech");
        if (mxInkConnected)
        {
            string hand = stylusOnRightHand ? "right hand" : "left hand";
            Debug.Log($"Device connected: {newDevice.name} on {hand}");
            stylus = newDevice;

            stylusOnRightHand = (newDevice.characteristics &
                InputDeviceCharacteristics.Right) != 0;
        }
    }
}
