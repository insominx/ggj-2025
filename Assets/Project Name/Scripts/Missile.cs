using UnityEngine;

public class Missile : MonoBehaviour
{

    public GameObject bomb;
    public float speed;
    public Vector3 targetDirection;
    private GameObject spawner;

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
        if (collision.gameObject != spawner)
        {
            Explode();
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        
        // Checks if the collider has the explosion script
        if(other.gameObject.GetComponent<Explosion>() != null && other.gameObject != spawner)
        {
            
            Explode();

        }

    }

    void Explode()
    {
        Instantiate(bomb, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    

}

