using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class ScaleWithUI : MonoBehaviour
    {
        [SerializeField] Canvas m_Canvas;

        private void Start() {
            SetLocalScaleToCanvasScale();
        }

        private void OnRectTransformDimensionsChange() {
            SetLocalScaleToCanvasScale();
        }

        private void SetLocalScaleToCanvasScale() {
            if (!m_Canvas) m_Canvas = GetComponentInParent<Canvas>();
            transform.localScale = new Vector3(1 / m_Canvas.transform.localScale.x, 1 / m_Canvas.transform.localScale.y, 1 / m_Canvas.transform.localScale.z);
        }
    }
}
