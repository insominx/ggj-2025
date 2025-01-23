using UnityEngine;

public class InputModalityManagerGGJ : MonoBehaviour
{
    public StylusEventRelay stylusEvents;
    public GameObject leftController;
    public GameObject rightController;

    //---------------------------------------------------------------------------
    void Start()
    {
        stylusEvents.onStylusConnected.AddListener(OnStylusConnected);
        stylusEvents.onStylusDisconnected.AddListener(OnStylusDisconnected);
    }

    //---------------------------------------------------------------------------
    void OnDestroy()
    {
        stylusEvents.onStylusConnected.AddListener(OnStylusConnected);
        stylusEvents.onStylusDisconnected.AddListener(OnStylusDisconnected);
    }

    //---------------------------------------------------------------------------
    void OnStylusDisconnected(StylusHandSide _)
    {
        leftController.SetActive(true);
        rightController.SetActive(true);
    }

    //---------------------------------------------------------------------------
    void OnStylusConnected(StylusHandSide handSide)
    {
        bool rightSide = handSide == StylusHandSide.Right;
        leftController.SetActive(rightSide);
        rightController.SetActive(!rightSide);
    }
}
