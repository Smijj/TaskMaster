using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class RectTransformTweenCtrl : MonoBehaviour
    {
        public RectTransform RTransform;


        private int m_MoveAnimTweenID = 0;
        public LTDescr MoveAnim(Vector2 from, Vector2 to, float animTime = 0.25f) {
            if (!RTransform) return null;
            LeanTween.cancel(m_MoveAnimTweenID);
            LTDescr tween = LeanTween.value(gameObject, x => RTransform.anchoredPosition = x, from, to, animTime);
            m_MoveAnimTweenID = tween.uniqueId;
            return tween;
        }

        private int m_RotateAnimTweenID = 0;
        public LTDescr RotateAnim(Vector3 from, Vector3 to, float animTime = 0.25f) {
            if (!RTransform) return null;
            LeanTween.cancel(m_RotateAnimTweenID);
            LTDescr tween = LeanTween.value(gameObject, x => RTransform.localEulerAngles = x, from, to, animTime);
            m_RotateAnimTweenID = tween.uniqueId;
            return tween;
        }

        private int m_ScaleAnimTweenID = 0;
        public LTDescr ScaleAnim(Vector2 from, Vector2 to, float animTime = 0.25f) {
            if (!RTransform) return null;
            LeanTween.cancel(m_ScaleAnimTweenID);
            LTDescr tween = LeanTween.value(gameObject, x => RTransform.sizeDelta = x, from, to, animTime);
            m_ScaleAnimTweenID = tween.uniqueId;
            return tween;
        }
        
    }
}
