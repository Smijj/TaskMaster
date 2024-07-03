using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.UIModule
{
    public class TaskItemCtrl : MonoBehaviour
    {
        [SerializeField] private Slider m_ImgCrossLine;
        [SerializeField] private TMP_Text m_TxtItemName;


        public void InitTask(string taskName) {
            LeanTween.cancel(m_CrossOffAnimID);
            if (m_ImgCrossLine) {
                m_ImgCrossLine.value = 0;
                m_ImgCrossLine.maxValue = 1;
            }
            if (m_TxtItemName) m_TxtItemName.text = "- " + taskName;
        }

        private int m_CrossOffAnimID = 0;
        public LTDescr CrossOffItemAnim(float animTime = 0.25f) {
            LeanTween.cancel(m_CrossOffAnimID);
            LTDescr tween = LeanTween.value(gameObject, x => m_ImgCrossLine.value = x, 0, 1, animTime);
            m_CrossOffAnimID = tween.uniqueId;
            return tween;
        }
    }
}
