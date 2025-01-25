using UnityEngine;

public class Living : MonoBehaviour
{

    public int health;

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
        
        // Checks if the collider has the explosion scrript
        if(other.gameObject.GetComponent<Explosion>() != null)
        {

            // If it does, the this object will take some damage
            this.takeDamage(1);

        }

    }

    // Lil function that handles the tanking of damage
    void takeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

}
