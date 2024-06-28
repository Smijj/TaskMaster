using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> m_MenuUIElements = new List<RectTransform>();
        [SerializeField] private List<RectTransform> m_GameplayUIElements = new List<RectTransform>();
        [SerializeField] private RectTransformTweenCtrl m_HeadBGCtrl;

        #region Unity + Events

        private void Start() {
            TransformBGToMenuPos(false);
        }
        private void OnEnable() {
            StatesModule.GameStates.OnInitGameplay += OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay += OnStartGameplay;
            StatesModule.GameStates.OnGameOver += OnGameOver;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnInitGameplay -= OnInitGameplay;
            StatesModule.GameStates.OnStartGameplay -= OnStartGameplay;
            StatesModule.GameStates.OnGameOver -= OnGameOver;
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TransformBGToGameplayPos();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TransformBGToMenuPos();
            }
        }

        [ContextMenu("TransformBGToGameplayPos")]
        private void OnInitGameplay() {
            // Hide Menu UI
            // Transform BG to Gamplay Pos
            TransformBGToGameplayPos();
        }
        private void OnStartGameplay() {
            // Show Gameplay UI
        }
        [ContextMenu("TransformBGToMenuPos")]
        private void OnGameOver() {
            // Hide Gameplay UI
            // Transform BG to Menu Pos
            TransformBGToMenuPos();
        }

        #endregion

        private void TransformBGToMenuPos(bool animate = true) {
            if (!m_HeadBGCtrl) return;
            Vector2 newPos = new Vector2(150, -130);
            Vector2 newSize = new Vector2(-200, 0);
            Vector3 newRot = new Vector3(0, 0, 277);

            if (!animate) {
                m_HeadBGCtrl.RTransform.anchoredPosition = newPos;
                m_HeadBGCtrl.RTransform.sizeDelta = newSize;
                m_HeadBGCtrl.RTransform.localEulerAngles = newRot;
                return;
            }

            m_HeadBGCtrl.MoveAnim(m_HeadBGCtrl.RTransform.anchoredPosition, newPos, 2);
            m_HeadBGCtrl.ScaleAnim(m_HeadBGCtrl.RTransform.sizeDelta, newSize, 2);
            m_HeadBGCtrl.RotateAnim(new Vector3(0, 0, 360), newRot, 2);
        }
        private void TransformBGToGameplayPos(bool animate = true) {
            if (!m_HeadBGCtrl) return;
            Vector2 newPos = Vector2.zero;
            Vector2 newSize = Vector2.zero;
            Vector3 newRot = new Vector3(0, 0, 360);

            if (!animate) {
                m_HeadBGCtrl.RTransform.anchoredPosition = newPos;
                m_HeadBGCtrl.RTransform.sizeDelta = newSize;
                m_HeadBGCtrl.RTransform.localEulerAngles = newRot;
                return;
            }

            m_HeadBGCtrl.MoveAnim(m_HeadBGCtrl.RTransform.anchoredPosition, newPos, 2);
            m_HeadBGCtrl.ScaleAnim(m_HeadBGCtrl.RTransform.sizeDelta, newSize, 2);
            m_HeadBGCtrl.RotateAnim(m_HeadBGCtrl.RTransform.localEulerAngles, newRot, 2);
        }
    }
}
