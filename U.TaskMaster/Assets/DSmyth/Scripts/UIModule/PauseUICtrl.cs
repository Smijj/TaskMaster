using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class PauseUICtrl : MonoBehaviour
    {
        [SerializeField] private CanvasGroupTweenCtrl m_PauseUICanvas;
        [SerializeField] private float m_FadeTime = 0.25f;

        private void Awake() {
            if (m_PauseUICanvas) m_PauseUICanvas.SetAlpha(0f);
        }
        private void OnEnable() {
            StatesModule.GameStates.OnGamePause += OnGamePause;
            StatesModule.GameStates.OnGameResume += OnGameResume;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnGamePause -= OnGamePause;
            StatesModule.GameStates.OnGameResume -= OnGameResume;
        }


        private void OnGamePause() {
            if (!m_PauseUICanvas) return;

            m_PauseUICanvas.gameObject.SetActive(true);
            m_PauseUICanvas.SetAlpha(0f);
            m_PauseUICanvas.AnimAlpha(0, 1, m_FadeTime);
        }
        private void OnGameResume() {
            if (!m_PauseUICanvas) return;

            m_PauseUICanvas.AnimAlpha(1, 0, m_FadeTime)
                .setOnComplete(() => m_PauseUICanvas.gameObject.SetActive(false));
        }
    }
}
