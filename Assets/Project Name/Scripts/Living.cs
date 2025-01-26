using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Living : MonoBehaviour
{
    
    // Health of the current building
    public int health;
    
    // Reference to a particle system to play once the object dies
    public GameObject deathParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        // Checks if the collider has the explosion script
        if(other.gameObject.GetComponent<Explosion>() != null)
        {

            // If it does, then this object will take some damage
            this.takeDamage(1);

        }

    }

    // Lil function that handles the tanking of damage
    void takeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            
            GameObject deathParticleObject = Instantiate(deathParticles, transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.up));
            deathParticleObject.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject);
            
        }

    }

}
