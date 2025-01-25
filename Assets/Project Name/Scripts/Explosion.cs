using UnityEngine;

public class Explosion : MonoBehaviour
{

    Vector3 scaleChange = new Vector3 (10f, 10f, 10f);

    // Makes sure the explosion explodes in 3 seconds
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.localScale += (scaleChange * Time.deltaTime);


    }
}
