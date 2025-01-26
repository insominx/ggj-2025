using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject bomb;
    public float speed;
    public Vector3 targetDirection;
    private GameObject spawner;

    public static int Count = 0;

    float maxLifetime = 15.0f; // Seconds.
    float startTime;

    public void Start()
    {
        targetDirection = transform.up.normalized;
        Count++;
        startTime = Time.time;
    }

    public void SteerTowards(Vector3 targetPosition)
    {
        targetDirection = (targetPosition - transform.position).normalized;
    }

    public void Update()
    {
        // Script to have the missile move to the target direction
        this.gameObject.transform.position += targetDirection * speed * Time.deltaTime;

        if (Time.time > startTime + maxLifetime)
        {
            // We've lived too long -- blow up.
            Explode();
        }
    }

    bool ExplodeTag(string tag)
    {
        switch (tag)
        {
        case "City":
        case "Bubble":
        case "Floor":
        case "Ceiling":
        case "Missile":
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Be more selective -- we only want to explode on hitting a) a target or b) a bubble.
        var other = collision.gameObject;
        if (other != spawner && ExplodeTag(other.tag))
        {
            Explode();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Checks if the collider has the explosion script
        if (other.gameObject.GetComponent<Explosion>() != null && other.gameObject != spawner)
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(bomb, this.transform.position, Quaternion.identity);
        Destroy(gameObject);
        Count--;
    }
}
