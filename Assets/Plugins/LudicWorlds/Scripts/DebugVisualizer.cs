using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DebugVisualizer : MonoBehaviour
{
    [FormerlySerializedAs("togglePlanesAction")]
    [Header("Input")]
    [SerializeField] InputActionReference toggleAction;
    [SerializeField] InputActionReference rightActivateAction;
    [SerializeField] InputActionReference leftActivateAction;

    [Header("References)")]
    [SerializeField] ARPlaneManager planeManager;
    [SerializeField] ARAnchorManager anchorManager;
    [SerializeField] ARBoundingBoxManager boundingBoxManager;
    [SerializeField] GameObject grabbableCubePrefab;
    [SerializeField] GameObject randoPrefab;
    // [SerializeField] XRRayInteractor leftRayInteractor;

    [Header("Properties")]
    int visualizationMode; // 0 = None, 1 = Planes, 2 = Boxes
    int numPlanesAddedOccurred;

    List<ARAnchor> anchors = new();

    //----------------------------------------------------------------------------
    void Start()
    {
        toggleAction.action.performed += OnToggleVisualization;
        planeManager.trackablesChanged.AddListener(OnPlanesChanged);
        anchorManager.trackablesChanged.AddListener(OnAnchorsChanged);
        boundingBoxManager.trackablesChanged.AddListener(OnBoundingBoxesChanged);
        rightActivateAction.action.performed += OnRightActivateActionPerformed;
        leftActivateAction.action.performed += OnLeftActivateActionPerformed;
    }

    //----------------------------------------------------------------------------
    void OnToggleVisualization(InputAction.CallbackContext obj)
    {
        SetVisualizationMode((visualizationMode + 1) % 3);
    }

    //----------------------------------------------------------------------------
    void SetVisualizationMode(int newMode)
    {
        visualizationMode = newMode;
        switch (visualizationMode)
        {
            case 1:
                SetPlanesVisibility(true);
                SetBoxesVisibility(false);
                break;
            case 2:
                SetPlanesVisibility(false);
                SetBoxesVisibility(true);
                break;
            default:
                SetPlanesVisibility(false);
                SetBoxesVisibility(false);
                break;
        }
    }

    //----------------------------------------------------------------------------
    void SetPlanesVisibility(bool isVisible)
    {
        float fillAlpha = isVisible ? 0.3f : 0f;
        float lineAlpha = isVisible ? 1f : 0f;

        foreach (ARPlane arPlane in planeManager.trackables)
            SetTrackableAlpha(arPlane, fillAlpha, lineAlpha);
    }

    //----------------------------------------------------------------------------
    void SetBoxesVisibility(bool isVisible)
    {
        float fillAlpha = isVisible ? 0.3f : 0f;
        float lineAlpha = isVisible ? 1f : 0f;

        foreach (ARBoundingBox arBoundingBox in boundingBoxManager.trackables)
            SetTrackableAlpha(arBoundingBox, fillAlpha, lineAlpha);
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
    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        if (args.added.Count > 0)
        {
            numPlanesAddedOccurred++;

            foreach (ARPlane plane in planeManager.trackables)
            {
                // string label = plane.classifications.ToString();
                // Debug.Log($"Plane ID: {plane.trackableId}, Label: {label}");
            }

            // Debug.Log($"-> Number of planes {planeManager.trackables.count}");
            // Debug.Log($"-> numPlanesAddedOccurred {numPlanesAddedOccurred}");
        }
        SetVisualizationMode(visualizationMode);
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
    void OnBoundingBoxesChanged(ARTrackablesChangedEventArgs<ARBoundingBox> args)
    {
        if (args.added.Count > 0)
        {
            foreach (ARBoundingBox box in boundingBoxManager.trackables)
            {
                string label = box.classifications.ToString();
                Debug.Log($"Box ID: {box.trackableId}, Label: {label}");
            }

            Debug.Log($"-> Number of boxes {boundingBoxManager.trackables.count}");
        }
        SetVisualizationMode(visualizationMode);
    }

    //----------------------------------------------------------------------------
    void OnRightActivateActionPerformed(InputAction.CallbackContext obj)
    {
        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.classifications.HasFlag(PlaneClassifications.Table))
            {
                GameObject newCube = Instantiate(grabbableCubePrefab);
                float cubeHeight = newCube.GetComponent<Collider>().bounds.size.y;
                newCube.transform.position = plane.transform.position + plane.transform.up * cubeHeight * 2f;
            }
        }
    }

    //----------------------------------------------------------------------------
    void OnLeftActivateActionPerformed(InputAction.CallbackContext obj)
    {
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
    }

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
        toggleAction.action.performed -= OnToggleVisualization;
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
        anchorManager.trackablesChanged.RemoveListener(OnAnchorsChanged);
        boundingBoxManager.trackablesChanged.RemoveListener(OnBoundingBoxesChanged);
        rightActivateAction.action.performed -= OnRightActivateActionPerformed;
        leftActivateAction.action.performed -= OnLeftActivateActionPerformed;
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
