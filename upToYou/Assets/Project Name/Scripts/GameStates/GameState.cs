// Base class for game states.
public abstract class GameState
{
    public string name;
    public abstract void Start();
    public abstract void Stop();
    public abstract void Update();
    public abstract bool ShouldEnd();

    public GameState(string name)
    {
        this.name = name;
    }
};
