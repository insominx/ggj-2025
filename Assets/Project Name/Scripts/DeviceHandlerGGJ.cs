using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeviceHandlerGGJ : MonoBehaviour
{
    [Header("Stylus")]
    public GameObject leftHandStylus;
    public GameObject rightHandStylus;

    // [Header("Controllers")]
    // public GameObject leftHandController;
    // public GameObject rightHandController;

    void Awake()
    {
        InputDevices.deviceConnected += DeviceConnected;
        InputDevices.deviceDisconnected += DeviceDisconnected;

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        foreach (InputDevice device in devices)
            DeviceConnected(device);
    }
    void OnDestroy()
    {
        InputDevices.deviceConnected -= DeviceConnected;
    }

    void DeviceDisconnected(InputDevice device)
    {
        // Debug.Log($"Device disconnected: {device.name}");
        bool mxInkDisconnected = device.name.ToLower().Contains("logitech");
        if (mxInkDisconnected)
        {
            leftHandStylus.SetActive(false);
            rightHandStylus.SetActive(false);
        }
    }
    void DeviceConnected(InputDevice device)
    {
        // Debug.Log($"Device connected: {device.name}");
        bool mxInkConnected = device.name.ToLower().Contains("logitech");
        if (mxInkConnected)
        {
            bool isOnRightHand = (device.characteristics & InputDeviceCharacteristics.Right) != 0;
            leftHandStylus.SetActive(!isOnRightHand);
            rightHandStylus.SetActive(isOnRightHand);

            MxInkHandler MxInkStylus = FindFirstObjectByType<MxInkHandler>();
            if (MxInkStylus)
            {
                MxInkStylus.SetHandedness(isOnRightHand);
                LineDrawing lineDrawing = FindFirstObjectByType<LineDrawing>();
                if (lineDrawing)
                {
                    lineDrawing.Stylus = MxInkStylus;
                }
            }
        }
    }
}
