using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(MeshRenderer))]
public class ARPlaneColorizer : MonoBehaviour
{
    ARPlane m_ARPlane;
    MeshRenderer m_PlaneMeshRenderer;

    //-----------------------------------------------------------------------
    void Awake()
    {
        m_ARPlane = GetComponent<ARPlane>();
        m_PlaneMeshRenderer = GetComponent<MeshRenderer>();
    }

    //-----------------------------------------------------------------------
    void Start()
    {
        UpdatePlaneColor();
    }

    //-----------------------------------------------------------------------
    void UpdatePlaneColor()
    {
        Color planeMatColor = GetColorForClassification(m_ARPlane.classifications);
        planeMatColor.a = 0.33f;
        m_PlaneMeshRenderer.material.color = planeMatColor;
    }

    //-----------------------------------------------------------------------
    Color GetColorForClassification(PlaneClassifications classifications)
    {
        if (classifications.HasFlag(PlaneClassifications.Floor)) return Color.green;
        if (classifications.HasFlag(PlaneClassifications.WallFace)) return Color.white;
        if (classifications.HasFlag(PlaneClassifications.Ceiling)) return Color.red;
        if (classifications.HasFlag(PlaneClassifications.Table)) return Color.yellow;
        if (classifications.HasFlag(PlaneClassifications.Seat)) return Color.blue;
        if (classifications.HasFlag(PlaneClassifications.DoorFrame)) return Color.magenta;
        if (classifications.HasFlag(PlaneClassifications.WindowFrame)) return Color.cyan;

        return Color.gray;
    }
}
