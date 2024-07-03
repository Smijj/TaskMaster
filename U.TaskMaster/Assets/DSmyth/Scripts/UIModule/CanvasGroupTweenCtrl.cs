using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class CanvasGroupTweenCtrl : MonoBehaviour
    {
        public CanvasGroup CvsGrp;

        private void Reset() {
            if (!CvsGrp) CvsGrp = GetComponent<CanvasGroup>();
        }

        public void SetAlpha(float alpha) {
            if (!CvsGrp) return;
            CvsGrp.alpha = alpha;
        }

        private int m_AlphaAnimTweenID = 0;
        public LTDescr AnimAlpha(float from, float to, float animTime = 0.25f) {
            if (!CvsGrp) return null;
            LeanTween.cancel(m_AlphaAnimTweenID);
            LTDescr tween = LeanTween.value(gameObject, x => CvsGrp.alpha = x, from, to, animTime);
            m_AlphaAnimTweenID = tween.uniqueId;
            return tween;
        }
    }
}
