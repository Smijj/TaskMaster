using System;

namespace DSmyth.StatesModule
{
    public static class GameStates
    {
        public static bool IsGamePlaying;

        public static Action OnInitGameplay;
        public static Action OnStartGameplay;
        public static Action OnGameOver;

        public static Action OnGamePause;
        public static Action OnGameResume;

        public static Action OnShoot;

        /// <summary>
        /// Passes through the Current Health percentage whenever the Current Health value changes.
        /// </summary>
        public static Action<float> OnHealthChanged;
        public static Action<int> OnScoreChanged;
        /// <summary>
        /// Returns a percentage between 0 and 1 that can be used to determine the current difficulty level.
        /// </summary>
        public static Action<float> OnDifficultyChanged;
    }
}
