using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayState : GameState
{
    GameManager gameManager;
    RealityMixer realityMixer;

    // State management.
    List<GameObject> cities = new();
    float timeWhenLastMissileSeen;

    public PlayState(GameManager gameManager)
        : base("Play")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        realityMixer = Object.FindFirstObjectByType<RealityMixer>();
        foreach (var pose in realityMixer.GetSpawnPoints())
        {
            var city = SpawnBase(pose);
            cities.Add(city);
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
        var cubeSideLength = 0.1f;
        cube.transform.localScale = new Vector3(cubeSideLength, cubeSideLength, cubeSideLength);
        cube.transform.SetPositionAndRotation(pose.position + new Vector3(0, cubeSideLength / 2, 0), pose.rotation);

        // Ready!
        return cube;
    }
}
