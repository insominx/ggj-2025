using System.Collections;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(CleanParticle());
    }

    IEnumerator CleanParticle()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
    
}
