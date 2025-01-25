using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DebugVisualizer : MonoBehaviour
{
    [Header("Configuration")]
    bool verboseLogging;

    [Header("References)")]
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] ARBoundingBoxManager boundingBoxManager;
    [SerializeField] GameObject grabbableCubePrefab;
    [SerializeField] GameObject randoPrefab;

    List<ARAnchor> anchors = new();

    //----------------------------------------------------------------------------
    void Start()
    {
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
        anchorManager.trackablesChanged.AddListener(OnAnchorsChanged);
        boundingBoxManager.trackablesChanged.AddListener(OnBoundingBoxesChanged);
    }

    //----------------------------------------------------------------------------
    void OnEnable()
    {
    }

    //----------------------------------------------------------------------------
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

        float fillAlpha = 0.3f;
        float lineAlpha = 1f;

        foreach (ARPlane arPlane in planeManager.trackables)
            SetTrackableAlpha(arPlane, fillAlpha, lineAlpha);
    }

    //----------------------------------------------------------------------------
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

        float fillAlpha = 0.3f;
        float lineAlpha = 1f;

        foreach (ARBoundingBox arBoundingBox in boundingBoxManager.trackables)
            SetTrackableAlpha(arBoundingBox, fillAlpha, lineAlpha);
    }

    //----------------------------------------------------------------------------
    void OnAnchorsChanged(ARTrackablesChangedEventArgs<ARAnchor> args)
    {
        foreach (KeyValuePair<TrackableId, ARAnchor> removedAnchor in args.removed)
        {
            if (removedAnchor.Value != null)
            {
                anchors.Remove(removedAnchor.Value);
                Destroy(removedAnchor.Value.gameObject);
            }
        }
    }

    //----------------------------------------------------------------------------
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

    //----------------------------------------------------------------------------
    // void OnRightActivateActionPerformed(InputAction.CallbackContext obj)
    // {
    //     foreach (ARPlane plane in planeManager.trackables)
    //     {
    //         if (plane.classifications.HasFlag(PlaneClassifications.Table))
    //         {
    //             GameObject newCube = Instantiate(grabbableCubePrefab);
    //             float cubeHeight = newCube.GetComponent<Collider>().bounds.size.y;
    //             newCube.transform.position = plane.transform.position + plane.transform.up * cubeHeight * 2f;
    //         }
    //     }
    // }

    //----------------------------------------------------------------------------
    // void OnLeftActivateActionPerformed(InputAction.CallbackContext obj)
    // {
        // if (leftRayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitInfo))
        // {
        //     Debug.Log($"-> Hit detected! - name: {hitInfo.transform.name}");
        //     Quaternion uprightRotation = Quaternion.LookRotation(hitInfo.normal, Vector3.up);
        //     // GameObject newObj = Instantiate(randoPrefab, hitInfo.point, uprightRotation);
        //     Pose pose = new(hitInfo.point, uprightRotation);
        //     CreateAnchorAsync(pose);
        //
        // }
        // else
        //     Debug.Log($"-> No hits detected");
    // }

    //----------------------------------------------------------------------------
    async void CreateAnchorAsync(Pose pose)
    {
        Result<ARAnchor> result = await anchorManager.TryAddAnchorAsync(pose);
        if (result.status.IsSuccess())
        {
            ARAnchor anchor = result.value;
            anchors.Add(anchor);

            GameObject newObj = Instantiate(randoPrefab, anchor.pose.position, anchor.pose.rotation);
            newObj.transform.SetParent(anchor.transform);
        }
    }

    //----------------------------------------------------------------------------
    void OnDestroy()
    {
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
        anchorManager.trackablesChanged.RemoveListener(OnAnchorsChanged);
        boundingBoxManager.trackablesChanged.RemoveListener(OnBoundingBoxesChanged);
    }

    //----------------------------------------------------------------------------
    void PrintTrackableInfo(ARTrackable trackable)
    {
        string classifer = "";
        if (trackable is ARPlane plane)
            classifer = plane.classifications.ToString();
        else if (trackable is ARBoundingBox box)
            classifer = box.classifications.ToString();

        string log = $"ID: {trackable.trackableId}, Classif: {classifer}";
        Debug.Log(log);
    }
}
