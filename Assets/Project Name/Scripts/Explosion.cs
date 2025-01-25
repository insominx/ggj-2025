using UnityEngine;

public class Explosion : MonoBehaviour
{

    Vector3 scaleChange = new Vector3 (10f, 10f, 10f);

    private bool growing = true;

    // Update is called once per frame
    void Update()
    {

        this.transform.localScale += (scaleChange * Time.deltaTime);

        if (growing)
        {
            if (this.transform.localScale.x > 15f)
            {
                scaleChange = new Vector3 (-10f, -10f, -10f);
                growing = false;
            }
        }
        else
        {
            if (this.transform.localScale.x < 1f)
            {
                Destroy(this.gameObject);
            }
        }


    }
}
