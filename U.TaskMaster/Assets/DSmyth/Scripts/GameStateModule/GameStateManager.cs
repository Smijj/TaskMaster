using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.GameStateModule
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("Config Settings")]
        [SerializeField] private float m_MaxHealth = 100;
        [SerializeField] private float m_TimeToMaxDifficulty = 120;     // After 2 minutes the game should be as hard as its going to get
        [SerializeField] private float m_DamageAmountTaskFailed = 10;
        [SerializeField] private float m_MinHealAmountTaskCompleted = 3;
        [SerializeField] private float m_MaxHealAmountTaskCompleted = 10;
        [ReadOnly, SerializeField] private float m_CurrentHealAmountTaskCompleted = 3;
        [SerializeField] private int m_ScoreAmountTaskCompleted = 10;
        [SerializeField] private int m_ScoreAmountEnemyDestroyed = 5;


        [Header("Game Data")]
        [ReadOnly, SerializeField] private float m_CurrentHealth = 0;
        public float CurrentHealth { 
            get => m_CurrentHealth; 
            private set {
                m_CurrentHealth = value;
                if (m_CurrentHealth <= 0) {
                    m_CurrentHealth = 0;
                    StatesModule.GameStates.OnGameOver?.Invoke(); // Game Over
                }
                else if (m_CurrentHealth > m_MaxHealth) m_CurrentHealth = m_MaxHealth;
                StatesModule.GameStates.OnHealthChanged?.Invoke(m_CurrentHealth / m_MaxHealth);
            }
        }

        [ReadOnly, SerializeField] private int m_CurrentScore = 0;
        public int CurrentScore {
            get => m_CurrentScore;
            private set {
                m_CurrentScore = value;
                if (m_CurrentScore > StorageModule.DataHandler.SaveData.HighScore) {
                    StorageModule.DataHandler.SaveData.HighScore = m_CurrentScore;
                }
                StatesModule.GameStates.OnScoreChanged?.Invoke(m_CurrentScore);
            }
        }

        [ReadOnly, SerializeField] private float m_ElapsedGameTime = 0;


        #region Unity + Events

        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay += OnStartGameplay;
            StatesModule.GameStates.OnGameOver += OnGameOver;

            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
            StatesModule.EnemyStates.OnDistractionDestroyed += OnDistractionDestroyed;
            StatesModule.TaskStates.OnTaskCompleted += OnTaskCompleted;
            StatesModule.TaskStates.OnTaskFailed += OnTaskFailed;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay -= OnStartGameplay;
            StatesModule.GameStates.OnGameOver -= OnGameOver;

            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
            StatesModule.EnemyStates.OnDistractionDestroyed -= OnDistractionDestroyed;
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
            float difficultyPercentage = m_ElapsedGameTime / m_TimeToMaxDifficulty;
            StatesModule.GameStates.OnDifficultyChanged?.Invoke(difficultyPercentage);

            m_CurrentHealAmountTaskCompleted = Mathf.Lerp(m_MinHealAmountTaskCompleted, m_MaxHealAmountTaskCompleted, difficultyPercentage);
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
        /// If the distraction has been destroyed by the player shooting a projectile at it increase the score by an amount
        /// </summary>
        private void OnDistractionDestroyed() {
            CurrentScore += m_ScoreAmountEnemyDestroyed;
        }

        /// <summary>
        /// Increase Current Score when a task is completed. Also Refill the Players Health by a small amount.
        /// </summary>
        private void OnTaskCompleted() {
            CurrentScore += m_ScoreAmountTaskCompleted;
            CurrentHealth += m_CurrentHealAmountTaskCompleted;
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
