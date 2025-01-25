using UnityEngine;

public class Missile : MonoBehaviour
{

    public GameObject bomb;
    public float speed;
    public Vector3 targetDirection;

    public void Start()
    {
        targetDirection = transform.up.normalized * speed;
    }
    
    public void Update()
    {
        // Script to have the missile move to the target direction
        this.gameObject.transform.position += targetDirection * Time.deltaTime;

    }
    
    void OnCollisionEnter(Collision collision)
    {

        Instantiate(bomb, this.transform.position, Quaternion.identity);
        Destroy(gameObject);

    }

}

