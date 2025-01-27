class DestroyEverythingState : GameState
{
    GameManager gameManager;

    public DestroyEverythingState(GameManager gameManager)
        : base("DestroyAll")
    {
        this.gameManager = gameManager;
    }

    public override void Start()
    {
        gameManager.DestroyAllCities();
        gameManager.DestroyAllEnemies();
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