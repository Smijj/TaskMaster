using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DSmyth.StatesModule;
using DSmyth.StorageModule;

namespace DSmyth.GameStateModule {
    public class StatsManager : MonoBehaviour
    {
        [Header("Comparable Stats")]
        [SerializeField] private TextMeshProUGUI m_HighScore;
        [SerializeField] private TextMeshProUGUI m_MostTasksCompeted;
        [SerializeField] private TextMeshProUGUI m_MostDistractionsDestroyed;
        [SerializeField] private TextMeshProUGUI m_LongestGameTime;

        [Header("Lifetime Stats")]
        [SerializeField] private TextMeshProUGUI m_TimesPlayed;
        [SerializeField] private TextMeshProUGUI m_TotalTasksCompleted;
        [SerializeField] private TextMeshProUGUI m_TotalDistractionsDestroyed;
        [SerializeField] private TextMeshProUGUI m_AverageReactionTime;

        [Header("Last Game Stats")]
        [SerializeField] private TextMeshProUGUI m_LastScore;
        [SerializeField] private TextMeshProUGUI m_LastTasksCompeted;
        [SerializeField] private TextMeshProUGUI m_LastDistractionsDestroyed;
        [SerializeField] private TextMeshProUGUI m_LastGameTime;

        

        #region Unity + Events

        private void Start() {
            UpdateStatsUI();
        }
        private void OnEnable() {
            GameStates.OnInitGameplay += OnInitGameplay;
            GameStates.OnGameOver += OnGameOver;
            TaskStates.OnTaskCompleted += OnTaskCompleted;
            EnemyStates.OnDistractionBlocked += OnDistractionBlocked;
            EnemyStates.OnDistractionDestroyed += OnDistractionDestroyed;
        }
        private void OnDisable() {
            GameStates.OnInitGameplay -= OnInitGameplay;
            GameStates.OnGameOver -= OnGameOver;
            TaskStates.OnTaskCompleted -= OnTaskCompleted;
            EnemyStates.OnDistractionBlocked -= OnDistractionBlocked;
            EnemyStates.OnDistractionDestroyed -= OnDistractionDestroyed;
        }

        private void OnInitGameplay() {
            // Increase m_TimesPlayed
            DataHandler.SaveData.TimesPlayed++;
        }
        private void OnGameOver() {
            // Save GameTime Stats
            if (GameStateManager.ElapsedGameTime > DataHandler.SaveData.LongestGameTime) {
                DataHandler.SaveData.LongestGameTime = GameStateManager.ElapsedGameTime;
            }

            // Save Last Game Stats
            DataHandler.SaveData.LastScore = GameStateManager.CurrentScore;
            DataHandler.SaveData.LastTasksCompeted = GameStateManager.CurrentTasksCompleted;
            DataHandler.SaveData.LastDistractionsDestroyed = GameStateManager.CurrentDistractionsDestroyed;
            DataHandler.SaveData.LastGameTime = GameStateManager.ElapsedGameTime;

            UpdateStatsUI();
        }
        private void OnTaskCompleted() {
            if (GameStateManager.CurrentTasksCompleted > DataHandler.SaveData.MostTasksCompeted) {
                DataHandler.SaveData.MostTasksCompeted = GameStateManager.CurrentTasksCompleted;
            }

            DataHandler.SaveData.TotalTasksCompleted++;
        }
        private void OnDistractionBlocked() {
            if (GameStateManager.CurrentDistractionsDestroyed > DataHandler.SaveData.MostDistractionsDestroyed) {
                DataHandler.SaveData.MostDistractionsDestroyed = GameStateManager.CurrentDistractionsDestroyed;
            }

            DataHandler.SaveData.TotalDistractionsDestroyed++;
        }
        private void OnDistractionDestroyed() {
            if (GameStateManager.CurrentDistractionsDestroyed > DataHandler.SaveData.MostDistractionsDestroyed) {
                DataHandler.SaveData.MostDistractionsDestroyed = GameStateManager.CurrentDistractionsDestroyed;
            }

            DataHandler.SaveData.TotalDistractionsDestroyed++;
        }

        #endregion

        private void UpdateStatsUI() {
            if (m_HighScore) m_HighScore.text = $"{DataHandler.SaveData.HighScore} | High Score";
            if (m_MostTasksCompeted) m_MostTasksCompeted.text = $"{DataHandler.SaveData.MostTasksCompeted} | Most Tasks Competed";
            if (m_MostDistractionsDestroyed) m_MostDistractionsDestroyed.text = $"{DataHandler.SaveData.MostDistractionsDestroyed} | Most Distractions Destroyed";
            if (m_LongestGameTime) m_LongestGameTime.text = $"{DataHandler.SaveData.LongestGameTime.ToString("0")}s | Longest Game Time";
            
            if (m_TimesPlayed) m_TimesPlayed.text = $"Day {DataHandler.SaveData.TimesPlayed}";
            if (m_TotalTasksCompleted) m_TotalTasksCompleted.text = $"{DataHandler.SaveData.TotalTasksCompleted} | Total Tasks Completed";
            if (m_TotalDistractionsDestroyed) m_TotalDistractionsDestroyed.text = $"{DataHandler.SaveData.TotalDistractionsDestroyed} | Total Distractions Destroyed";
            //if (m_AverageReactionTime) m_AverageReactionTime.text = $"{DataHandler.SaveData.AverageReactionTime} | Average ReactionTime";
            
            if (m_LastScore) m_LastScore.text = $"{DataHandler.SaveData.LastScore} | Score";
            if (m_LastTasksCompeted) m_LastTasksCompeted.text = $"{DataHandler.SaveData.LastTasksCompeted} | Tasks Competed";
            if (m_LastDistractionsDestroyed) m_LastDistractionsDestroyed.text = $"{DataHandler.SaveData.LastDistractionsDestroyed} | Distractions Destroyed";
            if (m_LastGameTime) m_LastGameTime.text = $"{DataHandler.SaveData.LastGameTime.ToString("0")}s | Game Time";
        }
    }
}
