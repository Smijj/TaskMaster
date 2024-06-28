using System;

namespace DSmyth.StatesModule
{
    public static class GameStates
    {
        public static bool IsGamePlaying;

        public static Action OnInitGameplay;
        public static Action OnStartGameplay;
        public static Action OnGameOver;

        /// <summary>
        /// Passes through the Current Health percentage whenever the Current Health value changes.
        /// </summary>
        public static Action<float> OnHealthChanged;
        public static Action<int> OnScoreChanged;
    }
}
