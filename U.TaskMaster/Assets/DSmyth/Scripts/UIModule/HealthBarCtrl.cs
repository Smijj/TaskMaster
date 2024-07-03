using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.UIModule
{
    public class HealthBarCtrl : MonoBehaviour
    {
        [SerializeField] private Slider m_FillSlider;


        private void Awake() {
            InitHeatlhBar();
        }
        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += InitHeatlhBar;
            StatesModule.GameStates.OnHealthChanged += SetHealthBar;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= InitHeatlhBar;
            StatesModule.GameStates.OnHealthChanged -= SetHealthBar;
        }

        private void InitHeatlhBar() {
            if (!m_FillSlider) return;
            // Init fill max to max health
            m_FillSlider.value = 1;
        }
        private void SetHealthBar(float healthPercentage) {
            if (!m_FillSlider) return;
            m_FillSlider.value = healthPercentage;
        }
    }
}
