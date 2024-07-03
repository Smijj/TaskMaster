using System;

namespace DSmyth.StatesModule {
    public static class EnemyStates {
        public static Action<int> OnDistractionSucessful;
        public static Action OnDistractionBlocked;
        public static Action OnDistractionDestroyed;
    }
}
