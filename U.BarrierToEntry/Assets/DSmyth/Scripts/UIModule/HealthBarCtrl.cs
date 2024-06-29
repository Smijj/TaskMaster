using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.UIModule
{
    public class HealthBarCtrl : MonoBehaviour
    {
        [SerializeField] private Image m_FillImage;


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
            if (!m_FillImage) return;
            // Init fill max to max health
            m_FillImage.fillAmount = 1;
        }
        private void SetHealthBar(float healthPercentage) {
            if (!m_FillImage) return;
            m_FillImage.fillAmount = healthPercentage;
        }
    }
}
