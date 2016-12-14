using System;
using System.Collections.Generic;
using static Asteroids.Model.AsteroidsGame;

namespace Asteroids.Model
{
    public class GameStateEventArgs : EventArgs
    {
        public EGameState GameState { get; private set; }

        public GameStateEventArgs(EGameState gameState)
        {
            GameState = gameState;
        }
    }
}
