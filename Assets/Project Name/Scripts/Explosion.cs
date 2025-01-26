using UnityEngine;

public class Explosion : MonoBehaviour
{
    //Vector3 scaleChange = new Vector3 (10f, 10f, 10f);
    Vector3 scaleChange = new Vector3 (0.1f, 0.1f, 0.1f);

    private bool growing = true;

    // Update is called once per frame
    void Update()
    {
        this.transform.localScale += (scaleChange * Time.deltaTime);

        if (growing)
        {
            //if (this.transform.localScale.x > 15f)
            if (this.transform.localScale.x > 0.2f)
            {
                //scaleChange = new Vector3 (-10f, -10f, -10f);
                scaleChange = new Vector3 (-0.2f, -0.2f, -0.2f);
                growing = false;
            }
        }
        else
        {
            //if (this.transform.localScale.x < 1f)
            if (this.transform.localScale.x < 0.01f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
