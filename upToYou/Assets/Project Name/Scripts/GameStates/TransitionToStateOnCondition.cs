//using System.Collections;
//using System.Collections.Generic;
using System;

class TransitionToStateOnCondition : GameState
{
    GameManager gameManager;
    GameState state;
    Func<bool> condition;

    public TransitionToStateOnCondition(GameManager gameManager, GameState state, Func<bool> condition)
        : base("Transition")
    {
        this.gameManager = gameManager;
        this.state = state;
        this.condition = condition;
    }

    public override void Start()
    {
        // Nada ftm.
    }

    public override void Stop()
    {
        if (condition())
        {
            gameManager.OverrideNextState(state);
        }
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