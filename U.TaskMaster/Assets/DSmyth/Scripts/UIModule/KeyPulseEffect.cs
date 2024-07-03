using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.UIModule
{
    public class KeyPulseEffect : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] private float m_PulseScale = 2f;
        [SerializeField] private float m_PulseAnimTime = 0.5f;
        [SerializeField] private Color m_PulseColour = Color.white;
        [SerializeField] private LeanTweenType m_ScaleEase = LeanTweenType.easeOutCirc;
        [SerializeField] private LeanTweenType m_FadeEase = LeanTweenType.easeInOutQuart;

        [Header("References")]
        [SerializeField] private Transform m_PulseParent;
        [SerializeField] private RectTransform m_PulseUIPrefab;

        private void OnEnable() {
            StatesModule.TaskStates.OnTaskCompleted += PulseEffect;
        }
        private void OnDisable() {
            StatesModule.TaskStates.OnTaskCompleted -= PulseEffect;
        }

        private void PulseEffect() {
            if (!m_PulseUIPrefab) return;
            
            var pulseUI = Instantiate(m_PulseUIPrefab, m_PulseParent);  // Instantiate PulseUI Object
            
            // Set the colour
            Image pulseImage = pulseUI.GetComponent<Image>();
            if (pulseImage) pulseImage.color = m_PulseColour;

            // Fade out over time
            CanvasGroup pulseCvsGrp = pulseUI.GetComponent<CanvasGroup>();
            if (pulseCvsGrp) pulseCvsGrp.LeanAlpha(0, m_PulseAnimTime).setEase(m_FadeEase);

            // Scale up over time then destroy the object
            LeanTween.scale(pulseUI, Vector3.one * m_PulseScale, m_PulseAnimTime)
                .setEase(m_ScaleEase)
                .setOnComplete(() => Destroy(pulseUI.gameObject));
        }
    }
}
