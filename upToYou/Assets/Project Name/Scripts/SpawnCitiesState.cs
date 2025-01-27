using UnityEngine;

class SpawnCitiesState : GameState
{
    GameManager gameManager;

    public SpawnCitiesState(GameManager gameManager)
        : base("SpawnCities")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        RealityMixer realityMixer = Object.FindFirstObjectByType<RealityMixer>();

        foreach (var pose in realityMixer.GetCitySpawnPoints())
        {
            var city = gameManager.SpawnBase(pose);
        }
    }

    public override void Stop()
    {
        // Nada ftm.
    }

    public override void Update()
    {
        // Nada ftm.
    }

    public override bool ShouldEnd()
    {
        return true;
    }
}