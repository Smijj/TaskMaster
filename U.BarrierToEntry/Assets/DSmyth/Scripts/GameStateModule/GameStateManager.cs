using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.GameStateModule
{
    public class GameStateManager : MonoBehaviour
    {
        [Header("Config Settings")]
        [SerializeField] private int m_MaxHealth = 100;
        [SerializeField] private int m_TaskCompletedHealAmount = 5;


        [Header("Game Data")]
        [ReadOnly, SerializeField] private int m_CurrentHealth = 0;
        public int CurrentHealth { 
            get => m_CurrentHealth; 
            private set {
                m_CurrentHealth = value;
                if (m_CurrentHealth < 0) m_CurrentHealth = 0;
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


        private void OnEnable() {
            StatesModule.GameStates.OnGameplayInititalized += OnGamePlayInititalized;

            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnGameplayInititalized -= OnGamePlayInititalized;

            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted -= OnTaskCompleted;
        }

        private void OnGamePlayInititalized() {
            m_CurrentHealth = m_MaxHealth;
            m_CurrentScore = 0;
        }


        /// <summary>
        /// Reduce Player Health if distraction is sucessful in hitting the Focus Zone
        /// </summary>
        private void OnDistractionSucessful(int enemyDamage) {
            CurrentHealth -= enemyDamage;
            if (m_CurrentHealth <= 0) {

                // Game Over
                StatesModule.GameStates.OnGameOver?.Invoke();
            }
        }

        /// <summary>
        /// Increase Current Score when a task is completed. Also Refill the Players Health by a small amount.
        /// </summary>
        private void OnTaskCompleted() {
            CurrentScore++;
            CurrentHealth += m_TaskCompletedHealAmount;
            if (CurrentScore > StorageModule.DataHandler.SaveData.HighScore) {
                StorageModule.DataHandler.SaveData.HighScore = CurrentScore;
            }
        }
    }
}
