using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class ScoreCtrl : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtScore;
        [SerializeField] private float m_ScorePulseScale = 1.3f;
        [SerializeField] private float m_ScorePulseInTime = 0.05f;
        [SerializeField] private float m_ScorePulseOutTime = 0.2f;

        private void Awake() {
            InitScore();
        }
        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += InitScore;
            StatesModule.GameStates.OnScoreChanged += OnScoreChanged;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= InitScore;
            StatesModule.GameStates.OnScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int score) {
            SetScore(score);
        }

        private void InitScore() {
            SetScore(0, false);
        }

        private void SetScore(int score, bool animate = true) {
            if (!m_TxtScore) return;
            if (!animate) {
                m_TxtScore.text = $"- {score} -";
                return;
            }

            ScaleScore(1, m_ScorePulseScale, m_ScorePulseInTime)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(() => {
                    m_TxtScore.text = $"- {score} -";
                    ScaleScore(m_ScorePulseScale, 1, m_ScorePulseOutTime)
                    .setEase(LeanTweenType.easeOutExpo);
                });
            
        }

        private int m_ScoreAnimTweenID = 0;
        private LTDescr ScaleScore(float from, float to, float animTime = 0.25f) {
            LeanTween.cancel(m_ScoreAnimTweenID);
            LTDescr tween = LeanTween.value(gameObject, x => m_TxtScore.transform.localScale = new Vector3(x, x, x), from, to, animTime);
            m_ScoreAnimTweenID = tween.uniqueId;
            return tween;
        }
    }
}
