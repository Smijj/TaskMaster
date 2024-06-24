using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.GameStateModule
{
    public class GameStateManager : MonoBehaviour
    {

        [SerializeField] private int m_PlayerCurrentHealth = 100;
        public int PlayerCurrentHealth { get => m_PlayerCurrentHealth; 
            private set {
                m_PlayerCurrentHealth = value;
                StatesModule.GameStates.OnHealthChanged?.Invoke(m_PlayerCurrentHealth);
            }
        }

        [SerializeField] private int m_CurrentScore = 0;
        public int CurrentScore {
            get => m_CurrentScore;
            private set {
                m_CurrentScore = value;
                StatesModule.GameStates.OnScoreChanged?.Invoke(m_CurrentScore);
            }
        }
        [SerializeField] private int m_TaskCompletedHealAmount = 5;


        private void OnEnable() {
            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted += OnTaskCompleted;
        }
        private void OnDisable() {
            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
            StatesModule.TaskStates.OnTaskCompleted -= OnTaskCompleted;
        }

        /// <summary>
        /// Reduce Player Health if distraction is sucessful in hitting the Focus Zone
        /// </summary>
        private void OnDistractionSucessful(int enemyDamage) {
            PlayerCurrentHealth -= enemyDamage;
            if (m_PlayerCurrentHealth <= 0) {
                PlayerCurrentHealth = 0;

                // Game Over
                StatesModule.GameStates.OnGameOver?.Invoke();
            }
        }

        /// <summary>
        /// Increase Current Score when a task is completed. Also Refill the Players Health by a small amount.
        /// </summary>
        private void OnTaskCompleted() {
            CurrentScore++;
            PlayerCurrentHealth += m_TaskCompletedHealAmount;
        }
    }
}
