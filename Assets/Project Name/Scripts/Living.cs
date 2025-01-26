using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Living : MonoBehaviour
{
    // Track all living things.
    public static List<Living> livingThings = new();

    // Health of the current building
    public int health;

    // Reference to a particle system to play once the object dies
    public GameObject deathParticles;

    public static Living GetRandomLivingThing()
    {
        int rand = UnityEngine.Random.Range(0, livingThings.Count);
        var thing = livingThings[rand];
        return thing;
    }

    void Awake()
    {
        livingThings.Add(this);
    }

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
        if (other.gameObject.GetComponent<Explosion>() != null)
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
            Die();
        }
    }

    void Die()
    {
        GameObject deathParticleObject = Instantiate(deathParticles, transform.position,
            Quaternion.FromToRotation(Vector3.forward, Vector3.up));
        deathParticleObject.GetComponent<ParticleSystem>().Play();
        livingThings.Remove(this);
        Destroy(this.gameObject);
    }

    public static void DestroyAll()
    {
        foreach (var thing in livingThings)
        {
            GameObject.Destroy(thing.gameObject);
        }
        livingThings.Clear();
    }
}
