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

    void takeDamage(int damage)
    {

        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

}
