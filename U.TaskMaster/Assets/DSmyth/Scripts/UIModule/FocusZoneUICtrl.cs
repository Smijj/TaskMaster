using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.UIModule
{
    public class FocusZoneUICtrl : MonoBehaviour
    {
        [SerializeField] private float m_DamageFlashAnimTime = 0.15f;
        [SerializeField] private Color m_DefaultColour = Color.white;
        [SerializeField] private Color m_DamageTakenColour = Color.red;
        [SerializeField] private Image m_ImgFocusZone;

        private void Awake() {
            if (m_ImgFocusZone) m_ImgFocusZone.color = m_DefaultColour;
        }
        private void OnEnable() {
            StatesModule.EnemyStates.OnDistractionSucessful += OnDistractionSucessful;
        }
        private void OnDisable() {
            StatesModule.EnemyStates.OnDistractionSucessful -= OnDistractionSucessful;
        }

        private void OnDistractionSucessful(int dmg) {
            if (!m_ImgFocusZone) return;
            FlashFocusZone(m_DamageTakenColour);
        }

        private int m_FlashFocusZoneTweenID = 0;
        private LTDescr FlashFocusZone(Color flashColour) {
            LeanTween.cancel(m_FlashFocusZoneTweenID);

            m_ImgFocusZone.color = m_DamageTakenColour;
            LTDescr tween = LeanTween.delayedCall(m_DamageFlashAnimTime, () => m_ImgFocusZone.color = m_DefaultColour);
            m_FlashFocusZoneTweenID = tween.uniqueId;
            return tween;
        }
    }
}
