using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage game state machine etc.
public class GameManager : MonoBehaviour
{
    // States.
    public GameState[] gameStates = new GameState[]
    {
        new StartState(),
        new InformState("Title", "Bubble Command", 10),
        new InformState("Ready", "Get ready...", 10),
        new PlayState()
    };

    // State management.
    int currentGameStateIdx = 0;
    GameState currentState { get { return gameStates[currentGameStateIdx]; } }

    // Public Unity interface. /////////////////////////////////////////////////

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
            // TODO: Update this in a better way (don't want to repeat start or title).
            currentGameStateIdx = (currentGameStateIdx + 1) % gameStates.Length;

            StartState(currentState);
        }
    }

    void StartState(GameState state)
    {
        Debug.Log("Game Manager starting game state " + state.name);
        state.Start();
    }
}
