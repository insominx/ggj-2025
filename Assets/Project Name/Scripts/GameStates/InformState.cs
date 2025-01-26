using UnityEngine;

class InformState : GameState
{
    // Configuration.
    string text;
    float timeoutSeconds;
    bool showCountdown;

    // Scratch memory.
    float startTime;

    public InformState(string name, string text, float timeoutSeconds, bool showCountdown = false)
        : base(name)
    {
        this.text = text;
        this.timeoutSeconds = timeoutSeconds;
        this.showCountdown = showCountdown;
    }

    public void SetText(string text)
    {
        this.text = text;
    }

    public override void Start()
    {
        startTime = Time.time;
        UpdateText();
    }

    public override void Stop()
    {
        Informer.ShowText("");
    }

    public override void Update()
    {
        if (showCountdown)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        var displayText = text;
        if (showCountdown)
        {
            displayText += "\n" + (int)(startTime + timeoutSeconds - Time.time);
        }
        Informer.ShowText(displayText);
    }

    public override bool ShouldEnd()
    {
        bool shouldEnd = Time.time - startTime > timeoutSeconds;
        return shouldEnd;
    }
}
