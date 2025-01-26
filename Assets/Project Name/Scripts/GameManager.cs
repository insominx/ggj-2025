using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage game state machine etc.
public class GameManager : MonoBehaviour
{
    // States.
    public GameState[] gameStates;
    InformState gameConclusion;

    // State management.
    int currentGameStateIdx = 0;
    GameState currentState { get { return gameStates[currentGameStateIdx]; } }

    // Public Unity interface. /////////////////////////////////////////////////

    void Awake()
    {
        gameStates = new GameState[]
        {
            new StartState(),
            new InformState("Wait", "", 5),
            new InformState("Title", "Bubble Command\nby\nBubbltroop", 10),
            new InformState("GetReady", "Get ready.", 10, true),
            new PlayState(this),
            gameConclusion = new InformState("Report", "<TBD>", 10),
        };
    }

    void Start()
    {
        // Nada ftm.
        StartState(currentState);
    }

    // Update is called once per frame.
    void Update()
    {
        currentState.Update();

        if (currentState.ShouldEnd())
        {
            StopState(currentState);

            // TODO: Update this in a better way (don't want to repeat start or title).
            currentGameStateIdx = (currentGameStateIdx + 1) % gameStates.Length;

            StartState(currentState);
        }
    }

    void StopState(GameState state)
    {
        state.Stop();
    }

    void StartState(GameState state)
    {
        Debug.Log("Game Manager starting game state " + state.name);
        state.Start();
    }

    public void ReportVictory()
    {
        gameConclusion.SetText("Victory!");
    }

    public void ReportDefeat()
    {
        gameConclusion.SetText("Defeat!");
    }
}
