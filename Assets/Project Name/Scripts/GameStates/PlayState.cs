using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class PlayState : GameState
{
    GameManager gameManager;

    const float maxRoundTime = 30.0f;

    // State management.
    float timeWhenLastMissileSeen;
    float timeWhenPlayStarted;

    public PlayState(GameManager gameManager)
        : base("Play")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        RealityMixer realityMixer = Object.FindFirstObjectByType<RealityMixer>();

        foreach (var pose in realityMixer.GetEnemySpawnPoints())
        {
            GameObject enemy = gameManager.SpawnEnemy(pose);
        }

        // Let's say we've seen a missile now, and we'll update this later.
        timeWhenPlayStarted = Time.time;
        timeWhenLastMissileSeen = timeWhenPlayStarted;

        // Start firing missiles now
        gameManager.StartFiringMissiles();

    }

    public override void Stop()
    {
        gameManager.StopFiringMissiles();
    }

    public override void Update()
    {
        if (gameManager.MissilesAreInTheAir())
        {
            timeWhenLastMissileSeen = Time.time;
        }

        if (Time.time > timeWhenPlayStarted + maxRoundTime)
        {
            gameManager.StopFiringMissiles();
        }
    }

    public override bool ShouldEnd()
    {
        // Two conditions for game being over -- either cities are all gone or waves are done.

        // Check for cities being all gone.
        var defeat = !Living.livingThings.Any();

        // We win if there have been no missiles for 5-10 seconds.
        var victory = Time.time - timeWhenLastMissileSeen > 7.5f;

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
