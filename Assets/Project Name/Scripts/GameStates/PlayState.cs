using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayState : GameState
{
    GameManager gameManager;
    RealityMixer realityMixer;

    // State management.
    List<GameObject> cities = new();
    List<GameObject> enemies = new();
    float timeWhenLastMissileSeen;

    public PlayState(GameManager gameManager)
        : base("Play")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        realityMixer = Object.FindFirstObjectByType<RealityMixer>();
        foreach (var pose in realityMixer.GetCitySpawnPoints())
        {
            var city = SpawnBase(pose);
            cities.Add(city);
        }

        foreach (var pose in realityMixer.GetEnemySpawnPoints())
        {
            var enemy = SpawnEnemy(pose);
            enemies.Add(enemy);
        }

        // Let's say we've seen a missile now, and we'll update this later.
        timeWhenLastMissileSeen = Time.time;
    }

    public override void Stop()
    {
        foreach (var city in cities)
        {
            GameObject.Destroy(city);
        }
        cities.Clear();

        foreach (var enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
        enemies.Clear();
    }

    public override void Update()
    {
        // Nada ftm.
    }

    public override bool ShouldEnd()
    {
        // Two conditions for game being over -- either cities are all gone or waves are done.

        // Check for cities being all gone.
        var defeat = !cities.Any();

        // We win if there have been no missiles for 5 or 10 seconds.
        var victory = Time.time - timeWhenLastMissileSeen > 10.0f;

        if (defeat)
        {
            gameManager.ReportDefeat();
        }
        else if (victory)
        {
            gameManager.ReportVictory();
        }

        return victory || defeat;
    }

    private GameObject SpawnBase(Pose pose)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Position and scale it.
        var sideLength = 0.1f;
        cube.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
        cube.transform.SetPositionAndRotation(pose.position + new Vector3(0, sideLength / 2, 0), pose.rotation);

        // Ready!
        return cube;
    }

    private GameObject SpawnEnemy(Pose pose)
    {
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Position and scale it.
        var sideLength = 0.1f;
        sphere.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
        sphere.transform.SetPositionAndRotation(pose.position + pose.forward * sideLength / 2, pose.rotation);

        // Ready!
        return sphere;
    }
}
