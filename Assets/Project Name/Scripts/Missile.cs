using UnityEngine;

public class Missile : MonoBehaviour
{

    public GameObject bomb;

    void OnCollisionEnter(Collision collision)
    {

        Instantiate(bomb, this.transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

}

