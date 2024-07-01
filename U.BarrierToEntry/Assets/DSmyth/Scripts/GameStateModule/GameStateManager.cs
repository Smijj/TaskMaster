using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.GameStateModule
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("Config Settings")]
        [SerializeField] private int m_MaxHealth = 100;
        [SerializeField] private float m_TimeToMaxDifficulty = 120;     // After 2 minutes the game should be as hard as its going to get
        [SerializeField] private int m_DamageAmountTaskFailed = 10;
        [SerializeField] private int m_HealAmountTaskCompleted = 3;
        [SerializeField] private int m_ScoreAmountTaskCompleted = 10;
        [SerializeField] private int m_ScoreAmountEnemyDestroyed = 5;


        [Header("Game Data")]
        [ReadOnly, SerializeField] private int m_CurrentHealth = 0;
        public int CurrentHealth { 
            get => m_CurrentHealth; 
            private set {
                m_CurrentHealth = value;
                if (m_CurrentHealth <= 0) {
                    m_CurrentHealth = 0;
                    StatesModule.GameStates.OnGameOver?.Invoke(); // Game Over
                }
                else if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;
                StatesModule.GameStates.OnHealthChanged?.Invoke((float)m_CurrentHealth / m_MaxHealth);
            }
        }

        [ReadOnly, SerializeField] private int m_CurrentScore = 0;
        public int CurrentScore {
            get => m_CurrentScore;
            private set {
                m_CurrentScore = value;
                StatesModule.GameStates.OnScoreChanged?.Invoke(value);
            }
        }

        [ReadOnly, SerializeField] private float m_ElapsedGameTime = 0;


        #region Unity + Events

        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay += OnStartGameplay;
            StatesModule.GameStates.OnGameOver += OnGameOver;

            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted += OnTaskCompleted;
            StatesModule.TaskStates.OnTaskFailed += OnTaskFailed;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay -= OnStartGameplay;
            StatesModule.GameStates.OnGameOver -= OnGameOver;

            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted -= OnTaskCompleted;
            StatesModule.TaskStates.OnTaskFailed -= OnTaskFailed;
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                StopGame();
            }

            HandleDifficulty();
        }
        private void HandleDifficulty() {
            if (!StatesModule.GameStates.IsGamePlaying || m_ElapsedGameTime >= m_TimeToMaxDifficulty) return;
            
            // Increase the current playtime if the game is actively playing
            m_ElapsedGameTime += Time.deltaTime;
            if (m_ElapsedGameTime >= m_TimeToMaxDifficulty) {
                StatesModule.GameStates.OnDifficultyChanged?.Invoke(1);
                return;
            }
            StatesModule.GameStates.OnDifficultyChanged?.Invoke(m_ElapsedGameTime / m_TimeToMaxDifficulty);
        }


        private void OnInitGameplay() {
            m_CurrentHealth = m_MaxHealth;
            m_CurrentScore = 0;
            m_ElapsedGameTime = 0;
        }
        private void OnStartGameplay() {
            StatesModule.GameStates.IsGamePlaying = true;
        }
        private void OnGameOver() {
            StatesModule.GameStates.IsGamePlaying = false;
        }

        /// <summary>
        /// Reduce Player Health if distraction is sucessful in hitting the Focus Zone
        /// </summary>
        private void OnDistractionSucessful(int enemyDamage) {
            CurrentHealth -= enemyDamage;
        }

        /// <summary>
        /// Increase Current Score when a task is completed. Also Refill the Players Health by a small amount.
        /// </summary>
        private void OnTaskCompleted() {
            CurrentScore += m_ScoreAmountTaskCompleted;
            CurrentHealth += m_HealAmountTaskCompleted;
            if (CurrentScore > StorageModule.DataHandler.SaveData.HighScore) {
                StorageModule.DataHandler.SaveData.HighScore = CurrentScore;
            }
        }
        private void OnTaskFailed() {
            CurrentHealth -= m_DamageAmountTaskFailed;
        }

        #endregion


        public void StartGame() {
            StatesModule.GameStates.OnInitGameplay?.Invoke();
        }
        public void StopGame() {
            StatesModule.GameStates.OnGameOver?.Invoke();

        }
    }
}
