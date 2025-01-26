using UnityEngine;

class InformState : GameState
{
    // Configuration.
    string text;
    float timeoutSeconds;

    // Scratch memory.
    float startTime;

    public InformState(string name, string text, float timeoutSeconds)
        : base(name)
    {
        this.text = text;
        this.timeoutSeconds = timeoutSeconds;
    }

    public override void Start()
    {
        startTime = Time.time;
        Informer.ShowText(text);
    }

    public override void Stop()
    {
        Informer.ShowText("");
    }

    public override void Update()
    {
        // Nada ftm.
    }

    public override bool ShouldEnd()
    {
        bool shouldEnd = Time.time - startTime > timeoutSeconds;
        return shouldEnd;
    }
}
