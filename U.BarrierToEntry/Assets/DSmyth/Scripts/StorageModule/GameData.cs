using UnityEngine;

namespace DSmyth.StorageModule {
    [System.Serializable]
    public class GameData {
        public static readonly string FilePath = Application.streamingAssetsPath + "/Data/GameData.json";

        [SerializeField] private int m_HighScore = 0;
        public int HighScore {
            get => m_HighScore;
            set {
                m_HighScore = value;
                DataHandler.Save(this, FilePath);
            }
        }
    }
}
