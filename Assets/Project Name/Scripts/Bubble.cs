using System.Collections;
//using UnityEditor.Rendering;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Game Object will destroy itself at some point dependent on it's size
    IEnumerator DestroyBubble()
    {
        var lifetime = gameObject.transform.localScale.x * 5f;
        Debug.Log("Bubble lifetime is " + lifetime);
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    public void PrepareToDie()
    {
        // No, Mr. Bond. I expect you to die.
        StartCoroutine(DestroyBubble());
    }
}
