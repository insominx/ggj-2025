using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

public class StylusHapticsTest : MonoBehaviour
{
    [Header("Configuration")]
    public bool demoPeriodicHaptic;

    InputDevice stylus;

    void Awake()
    {
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

    void OnInputDeviceConnected(InputDevice newDevice)
    {
        bool mxInkConnected = newDevice.name.ToLower().Contains("logitech");
        if (mxInkConnected)
            stylus = newDevice;
    }
}
