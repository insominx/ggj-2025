using UnityEngine;

public class StylusEventLogger : MonoBehaviour
{
    [Header("Stylus Events Source")]
    [Tooltip("Reference to the StylusEventsGGJ component.")]
    public StylusEventRelay stylusEvents;

    [Header("Logging Options")]
    [Tooltip("Enable logging for Tip Action.")]
    public bool logTipAction = true;

    [Tooltip("Enable logging for Grab Action.")]
    public bool logGrabAction = true;

    [Tooltip("Enable logging for Option Action.")]
    public bool logOptionAction = true;

    [Tooltip("Enable logging for Middle Action.")]
    public bool logMiddleAction = true;

    //---------------------------------------------------------------------------
    void Reset()
    {
        // Automatically find StylusEventsGGJ in the same GameObject if not assigned
        if (stylusEvents == null)
            stylusEvents = GetComponent<StylusEventRelay>();
    }

    //---------------------------------------------------------------------------
    void OnEnable()
    {
        if (stylusEvents == null)
        {
            Debug.LogError("StylusEventsGGJ reference is not set. Please assign it in the inspector.");
            return;
        }

        // Subscribe to StylusEventsGGJ UnityEvents
        stylusEvents.onTipActionPerformed.AddListener(OnTipActionPerformed);
        stylusEvents.onGrabActionPerformed.AddListener(OnGrabActionPerformed);
        stylusEvents.onOptionActionPerformed.AddListener(OnOptionActionPerformed);
        stylusEvents.onMiddleActionPerformed.AddListener(OnMiddleActionPerformed);
    }

    //---------------------------------------------------------------------------
    void OnDisable()
    {
        if (stylusEvents == null)
            return;

        // Unsubscribe from StylusEventsGGJ UnityEvents
        stylusEvents.onTipActionPerformed.RemoveListener(OnTipActionPerformed);
        stylusEvents.onGrabActionPerformed.RemoveListener(OnGrabActionPerformed);
        stylusEvents.onOptionActionPerformed.RemoveListener(OnOptionActionPerformed);
        stylusEvents.onMiddleActionPerformed.RemoveListener(OnMiddleActionPerformed);
    }

    //---------------------------------------------------------------------------
    // Event Handlers with Logging Toggles

    /// <summary>
    /// Handles the Tip Action Performed event.
    /// </summary>
    /// <param name="tipValue">The value representing tip pressure.</param>
    public void OnTipActionPerformed(float tipValue)
    {
        if (logTipAction)
        {
            Debug.Log($"[LogStylusEvents] Tip Action Performed: Value = {tipValue}");
        }
    }

    /// <summary>
    /// Handles the Grab Action Performed event.
    /// </summary>
    /// <param name="isGrabPressed">Boolean indicating if the grab button is pressed.</param>
    public void OnGrabActionPerformed(bool isGrabPressed)
    {
        if (logGrabAction)
        {
            Debug.Log($"[LogStylusEvents] Grab Action Performed: IsPressed = {isGrabPressed}");
        }
    }

    /// <summary>
    /// Handles the Option Action Performed event.
    /// </summary>
    /// <param name="isOptionPressed">Boolean indicating if the option button is pressed.</param>
    public void OnOptionActionPerformed(bool isOptionPressed)
    {
        if (logOptionAction)
        {
            Debug.Log($"[LogStylusEvents] Option Action Performed: IsPressed = {isOptionPressed}");
        }
    }

    /// <summary>
    /// Handles the Middle Action Performed event.
    /// </summary>
    /// <param name="middleValue">The value representing the middle button's analog input.</param>
    public void OnMiddleActionPerformed(float middleValue)
    {
        if (logMiddleAction)
        {
            Debug.Log($"[LogStylusEvents] Middle Action Performed: Value = {middleValue}");
        }
    }
}
