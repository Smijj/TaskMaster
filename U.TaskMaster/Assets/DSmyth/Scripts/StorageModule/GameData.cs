using UnityEngine;

namespace DSmyth.StorageModule {
    [System.Serializable]
    public class GameData {
        public static readonly string FilePath = Application.streamingAssetsPath + "/Data/GameData.json";


        // ========= Comparable Stats

        [SerializeField] private int m_HighScore = 0;
        public int HighScore {
            get => m_HighScore;
            set {
                m_HighScore = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_MostTasksCompeted = 0;
        public int MostTasksCompeted {
            get => m_MostTasksCompeted;
            set {
                m_MostTasksCompeted = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_MostDistractionsDestroyed = 0;
        public int MostDistractionsDestroyed {
            get => m_MostDistractionsDestroyed;
            set {
                m_MostDistractionsDestroyed = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private float m_LongestGameTime = 0;
        public float LongestGameTime {
            get => m_LongestGameTime;
            set {
                m_LongestGameTime = value;
                DataHandler.Save(this, FilePath);
            }
        }


        // ========== Lifetime Stats

        [SerializeField] private int m_TimesPlayed = 0;
        public int TimesPlayed {
            get => m_TimesPlayed;
            set {
                m_TimesPlayed = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_TotalTasksCompleted = 0;
        public int TotalTasksCompleted {
            get => m_TotalTasksCompleted;
            set {
                m_TotalTasksCompleted = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_TotalDistractionsDestroyed = 0;
        public int TotalDistractionsDestroyed {
            get => m_TotalDistractionsDestroyed;
            set {
                m_TotalDistractionsDestroyed = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private float m_AverageReactionTime = 0;
        public float AverageReactionTime {
            get => m_AverageReactionTime;
            set {
                m_AverageReactionTime = value;
                DataHandler.Save(this, FilePath);
            }
        }


        // ====== Last Run Stats

        [SerializeField] private int m_LastScore = 0;
        public int LastScore {
            get => m_LastScore;
            set {
                m_LastScore = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_LastTasksCompeted = 0;
        public int LastTasksCompeted {
            get => m_LastTasksCompeted;
            set {
                m_LastTasksCompeted = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private int m_LastDistractionsDestroyed = 0;
        public int LastDistractionsDestroyed {
            get => m_LastDistractionsDestroyed;
            set {
                m_LastDistractionsDestroyed = value;
                DataHandler.Save(this, FilePath);
            }
        }
        [SerializeField] private float m_LastGameTime = 0;
        public float LastGameTime {
            get => m_LastGameTime;
            set {
                m_LastGameTime = value;
                DataHandler.Save(this, FilePath);
            }
        }
    }
}
