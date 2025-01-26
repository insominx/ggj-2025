using System.Collections;
using UnityEditor.Rendering;
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
        yield return new WaitForSeconds(this.gameObject.transform.localScale.x * 5f);
        Destroy(this.gameObject);
    }
    
}
