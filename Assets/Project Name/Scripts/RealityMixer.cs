using UnityEngine;

// Class to aid in establishing the correlation of the mixed reality coordinate system.
// This defines the coordinate bases and provides a means for the player to communicate them.
public class RealityMixer : MonoBehaviour
{
    // Editor/data/prefab interface.
    public GameObject floor;
    public GameObject leftController;
    public GameObject rightController;

    // Internal state.
    private Vector3 prevLeftControllerPos;
    private Vector3 prevRightControllerPos;

    void Start()
    {
        // Nada ftm.
    }

    private static bool Different(Vector3 a, Vector3 b)
    {
        return Vector3.Distance(a, b) > 0.0f;
    }

    // Update is called once per frame.
    void Update()
    {
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
    }
}
