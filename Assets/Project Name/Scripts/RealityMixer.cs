using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Class to aid in establishing the correlation of the mixed reality coordinate system.
// This defines the coordinate bases and provides a means for the player to communicate them.
public class RealityMixer : MonoBehaviour
{
    [Header("Configuration")]
    bool verboseLogging = false;

    // Editor/data/prefab interface.
    [Header("References)")]
    //[SerializeField] GameObject floor;
    //[SerializeField] GameObject leftController;
    //[SerializeField] GameObject rightController;
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARBoundingBoxManager boundingBoxManager;

    // Internal state.
    private Vector3 prevLeftControllerPos;
    private Vector3 prevRightControllerPos;

    List<Pose> spawnPoints = new();

    void Start()
    {
        // find Trackables
        // Look at DebugVisualizer as an example.
        // Get the planeManager and the boundingBoxManager.
        // Can subscribe to events to know when they come into existence.
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
        boundingBoxManager.trackablesChanged.AddListener(OnBoundingBoxesChanged);
    }

    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (args.added.Count > 0 && verboseLogging)
        {
            foreach (ARPlane plane in planeManager.trackables)
            {
                string label = plane.classifications.ToString();
                Debug.Log($"Plane ID: {plane.trackableId}, Label: {label}");
            }

            Debug.Log($"-> Number of planes {planeManager.trackables.count}");
        }

        float fillAlpha = 0.01f; // 0.3f;
        float lineAlpha = 0.1f; // 1f;

        foreach (ARPlane arPlane in planeManager.trackables)
        {
            SetTrackableAlpha(arPlane, fillAlpha, lineAlpha);
        }
    }

    void OnBoundingBoxesChanged(ARTrackablesChangedEventArgs<ARBoundingBox> args)
    {
        if (args.added.Count > 0 && verboseLogging)
        {
            foreach (ARBoundingBox box in boundingBoxManager.trackables)
            {
                string label = box.classifications.ToString();
                Debug.Log($"Box ID: {box.trackableId}, Label: {label}");
            }

            Debug.Log($"-> Number of boxes {boundingBoxManager.trackables.count}");
        }

        float fillAlpha = 0.01f; // 0.3f;
        float lineAlpha = 0.1f; // 1f;

        spawnPoints.Clear();
        foreach (ARBoundingBox arBoundingBox in boundingBoxManager.trackables)
        {
            SetTrackableAlpha(arBoundingBox, fillAlpha, lineAlpha);

            // The top of each bounding box is a reasonable place to set a base (Michael's suggestion).
            var pose = arBoundingBox.pose;
            var size = arBoundingBox.size;
            var spot = pose.position + pose.up * size.y / 2;

            spawnPoints.Add(new Pose(spot, pose.rotation));
        }
    }

    private static bool Different(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) > 0.0f;
    }

    // Update is called once per frame.
    void Update()
    {
        /*
        // Read current controller poses.
        var currLeftControllerPos = leftController.transform.position;
        var currRightControllerPos = rightController.transform.position;

        // Only trust the positions of the controllers if they're different from last time.
        var trustLeftPos = Different(currLeftControllerPos, prevLeftControllerPos);
        var trustRightPos = Different(currRightControllerPos, prevRightControllerPos);

        // Let trusted controllers push the floor downward.
        var newY = floor.transform.position.y;
        if (trustLeftPos)
        {
            newY = Mathf.Min(newY, currLeftControllerPos.y);
        }
        if (trustRightPos)
        {
            newY = Mathf.Min(newY, currRightControllerPos.y);
        }
        if (floor.transform.position.y > newY)
        {
            Debug.Log("Pushing floor from " + floor.transform.position.y + " to " + newY);
            floor.transform.position = new Vector3(
                floor.transform.position.x,
                newY,
                floor.transform.position.z);
        }

        // Remember hand positions for next time.
        prevLeftControllerPos = currLeftControllerPos;
        prevRightControllerPos = currRightControllerPos;
        //*/
    }

    void SetTrackableAlpha(ARTrackable trackable, float fillAlpha, float lineAlpha)
    {
        MeshRenderer meshRenderer = trackable.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            Color color = meshRenderer.material.color;
            color.a = fillAlpha;
            meshRenderer.material.color = color;
        }
        else
            Debug.Log($"Can't find MeshRenderer on trackable: {trackable.trackableId}");

        LineRenderer lineRenderer = trackable.GetComponentInChildren<LineRenderer>();
        if (lineRenderer != null)
        {
            Color startColor = lineRenderer.startColor;
            Color endColor = lineRenderer.endColor;
            startColor.a = endColor.a = lineAlpha;
            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;
        }
    }
}
