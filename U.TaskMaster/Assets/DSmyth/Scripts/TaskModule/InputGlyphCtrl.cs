using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DSmyth.TaskModule {
    public class InputGlyphCtrl : MonoBehaviour {
        [SerializeField] private Image m_ImgInputGlyph;

        private void Reset() {
            if (!m_ImgInputGlyph) m_ImgInputGlyph = GetComponent<Image>();
        }
        private void Awake() {
            if (!m_ImgInputGlyph) m_ImgInputGlyph = GetComponent<Image>();
        }

        public void SetImageSprite(Sprite sprite) {
            if (!m_ImgInputGlyph) return;
            m_ImgInputGlyph.sprite = sprite;
        }
        public void SetImageColour(Color colour) {
            if (!m_ImgInputGlyph) return;
            m_ImgInputGlyph.color = colour;
        }
    }
}