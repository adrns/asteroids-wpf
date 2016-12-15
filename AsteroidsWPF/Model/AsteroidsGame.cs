using System.Collections.Generic;
using System.Timers;
using System;
using System.Diagnostics;

namespace Asteroids.Model
{
    public class AsteroidsGame
    {
        private IGameRules gameRules;
        private SpaceShip player;
        private List<Asteroid> asteroids = new List<Asteroid>(20);
        private Timer timer;
        private Random random = new Random();
        private Stopwatch stopWatch;
        public enum EGameState { Running, Paused, GameOver }

        private EGameState gameState;
        public EGameState GameState
        {
            get { return gameState; }
            private set { gameState = value; OnGameStateChange?.Invoke(this, new GameStateEventArgs(gameState)); }
        }
        private int viewUpdateInterval;
        private long lastUpdate;
        private bool movingLeft = false;
        private bool movingRight = false;
        private bool movingUp = false;
        private bool movingDown = false;

        public delegate void FrameUpdateHandler(object sender, FrameEventArgs e);
        public event FrameUpdateHandler OnFrameUpdate;

        public delegate void GameStateHandler(object sender, GameStateEventArgs e);
        public event GameStateHandler OnGameStateChange;

        public AsteroidsGame(IGameRules gameRules, int fps)
        {
            this.gameRules = gameRules;
            viewUpdateInterval = 0 == fps ? 0 : 1000 / fps;
            timer = new Timer(gameRules.SpeedMs);
            timer.Elapsed += gameLoop;
            stopWatch = new Stopwatch();
            GameState = EGameState.GameOver;
        }

        public void start()
        {
            if (GameState == EGameState.GameOver)
            {
                GameState = EGameState.Running;
                asteroids.Clear();
                player = new SpaceShip(gameRules);
                timer.Start();
                stopWatch.Restart();
                lastUpdate = stopWatch.ElapsedMilliseconds;
            }
        }

        public void resume()
        {
            if (GameState == EGameState.Paused)
            {
                GameState = EGameState.Running;
                timer.Start();
                stopWatch.Start();
            }
        }

        public void pause()
        {
            if (GameState == EGameState.Running)
            {
                GameState = EGameState.Paused;
                timer.Stop();
                stopWatch.Stop();
            }
        }

        private void gameLoop(object sender, ElapsedEventArgs e)
        {
            despawnObjects();
            advanceObjects();
            spawnObjects();
            if (playerCollided()) gameOver(); else updateView();
        }

        private void updateView()
        {
            long now = stopWatch.ElapsedMilliseconds;
            if (viewUpdateInterval < now - lastUpdate)
            {
                lastUpdate = now;
                OnFrameUpdate?.Invoke(this, new FrameEventArgs(player, asteroids, stopWatch.ElapsedMilliseconds / 1000L));
            }
        }

        private void despawnObjects()
        {
            asteroids.RemoveAll(asteroid => gameRules.isOutOfBounds(asteroid));
        }

        private void advanceObjects()
        {
            acceleratePlayer();
            player.advance();
            foreach (Asteroid asteroid in asteroids)
                asteroid.advance();
        }

        private void spawnObjects()
        {
            if (gameRules.canSpawn(stopWatch.ElapsedMilliseconds))
                asteroids.Add(gameRules.getNextAsteroid());
        }

        private void acceleratePlayer()
        {
            if (movingLeft) player.thrustLeft();
            if (movingRight) player.thrustRight();
            if (movingUp) player.thrustUp();
            if (movingDown) player.thrustDown();
        }

        private bool playerCollided()
        {
            foreach (Asteroid asteroid in asteroids)
                if (asteroid.collidesWith(player))
                    return true;

            return false;
        }

        private void gameOver()
        {
            timer.Stop();
            stopWatch.Stop();
            GameState = EGameState.GameOver;
        }

        public void leftPressed() { movingLeft = true; }
        public void rightPressed() { movingRight = true; }
        public void leftReleased() { movingLeft = false; }
        public void rightReleased() { movingRight = false; }
        public void upPressed() { movingUp = true; }
        public void downPressed() { movingDown = true; }
        public void upReleased() { movingUp = false; }
        public void downReleased() { movingDown = false; }
    }
}
