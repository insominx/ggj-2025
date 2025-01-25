using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

// Manage game state machine etc.
public class GameManager : MonoBehaviour
{
    float timeOut;

    // Base class for game states.
    public abstract class GameState
    {
        public string name;
        public abstract void Start();
        public abstract void Update();
        public abstract bool ShouldEnd();

        public GameState(string name)
        {
            this.name = name;
        }
    };

    // Derived Game States.
    class StartGameState : GameState
    {
        public StartGameState()
            : base("Start")
        {
            // Nada ftm.
        }

        public override void Start()
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

    // The array!
    public GameState[] gameStates = new GameState[] {
        new StartGameState()
    };
    int currentGameState = 0;

    // Public Unity interface. /////////////////////////////////////////////////

    void Start()
    {
        // Nada ftm.
        GameState current = gameStates[currentGameState];
        Debug.Log("Game Manager starting game state " + current.name);
        current.Start();
    }

    // Update is called once per frame.
    void Update()
    {
        GameState current = gameStates[currentGameState];

        current.Update();

        if (current.ShouldEnd())
        {
            // TODO: move along...
        }
    }
}
