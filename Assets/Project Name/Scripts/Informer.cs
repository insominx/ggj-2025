using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Show messages to the user.
public class Informer : MonoBehaviour
{
    static Informer instance;

    // Editor interface.
    public TextMeshPro textDisplay;

    // Scratch variables.
    private Camera mainCamera;

    // Public application interface. ///////////////////////////////////////////
    public static void ShowText(string message)
    {
        if (!instance) { return; }

        instance.ApplyText(message);
    }

    // Public Unity interface. /////////////////////////////////////////////////

    void Start()
    {
        // Find thyself.
        instance = UnityEngine.Object.FindFirstObjectByType<Informer>();
        if (!instance)
        {
            WarnMissing("instance of type Informer");
        }

        // Find main camera.
        mainCamera = Camera.main;
        if (!instance)
        {
            // Bail with warning.
            WarnMissing("main camera");
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // Todo: orient towards the main camera.
        if (!mainCamera) { return; }

        transform.LookAt(mainCamera.transform);
    }

    // Private utility methods. ////////////////////////////////////////////////
    private void WarnMissing(string thing)
    {
        Debug.Log("Warning: could not find " + thing + ". Maybe you need to add it to your scene?");
    }

    private void ApplyText(string message)
    {
        textDisplay?.SetText(message);
    }
}
