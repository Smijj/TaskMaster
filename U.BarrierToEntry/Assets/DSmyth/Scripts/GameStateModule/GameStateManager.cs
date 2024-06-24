using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.GameStateModule
{
    public class GameStateManager : MonoBehaviour
    {

        [SerializeField] private int m_PlayerCurrentHealth = 100;
        


        private void OnEnable() {
            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
        }
        private void OnDisable() {
            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
        }

        /// <summary>
        /// Reduce Player Health if distraction is sucessful
        /// </summary>
        private void OnDistractionSucessful() {

        }
    }
}
