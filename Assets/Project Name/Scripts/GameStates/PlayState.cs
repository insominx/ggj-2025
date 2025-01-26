using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayState : GameState
{
    GameManager gameManager;
    RealityMixer realityMixer;

    // State management.
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
            var city = gameManager.SpawnBase(pose);
        }

        foreach (var pose in realityMixer.GetEnemySpawnPoints())
        {
            GameObject enemy = gameManager.SpawnEnemy(pose);
        }

        // Let's say we've seen a missile now, and we'll update this later.
        timeWhenLastMissileSeen = Time.time;
    }

    public override void Stop()
    {
        gameManager.DestroyAllCities();
        gameManager.DestroyAllEnemies();
    }

    public override void Update()
    {
        // Nada ftm.
    }

    public override bool ShouldEnd()
    {
        // Two conditions for game being over -- either cities are all gone or waves are done.

        // Check for cities being all gone.
        var defeat = !gameManager.cities.Any();

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
}
