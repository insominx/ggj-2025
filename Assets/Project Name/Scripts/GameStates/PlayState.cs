using UnityEngine;

class PlayState : GameState
{
    GameManager gameManager;
    RealityMixer realityMixer;

    public PlayState(GameManager gameManager)
        : base("Play")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        realityMixer = Object.FindFirstObjectByType<RealityMixer>();
        var cubeSideLength = 0.1f;
        foreach (var pose in realityMixer.GetSpawnPoints())
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = new Vector3(cubeSideLength, cubeSideLength, cubeSideLength);
            cube.transform.SetPositionAndRotation(pose.position + new Vector3(0, cubeSideLength / 2, 0), pose.rotation);
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
        return false;
    }
}
