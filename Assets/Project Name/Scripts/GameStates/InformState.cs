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
    }

    public override void Update()
    {
        // TODO: show on-screen text.
    }

    public override bool ShouldEnd()
    {
        bool shouldEnd = Time.time - startTime > timeoutSeconds;
        return shouldEnd;
    }
}
