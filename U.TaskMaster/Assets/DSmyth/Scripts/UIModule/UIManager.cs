using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSmyth.UIModule
{
    public class UIManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_UIFadeTime = 0.25f;
        [SerializeField] private LeanTweenType m_UIFadeEase = LeanTweenType.easeInQuad;
        [SerializeField] private float m_BGMoveAnimTime = 0.5f;
        [SerializeField] private LeanTweenType m_BGMoveEase = LeanTweenType.easeInCubic;
        [SerializeField] private float m_BGScaleAnimTime = 1f;
        [SerializeField] private LeanTweenType m_BGScaleEase = LeanTweenType.easeOutBounce;
        [SerializeField] private float m_BGRotateAnimTime = 1.5f;
        [SerializeField] private LeanTweenType m_BGRotateEase = LeanTweenType.easeOutBounce;

        [Header("References")]
        [SerializeField] private RectTransformTweenCtrl m_HeadBGCtrl;
        [SerializeField] private CanvasGroupTweenCtrl[] m_MenuUIElements;
        [SerializeField] private CanvasGroupTweenCtrl[] m_GameplayUIElements;
        [SerializeField] private CanvasGroupTweenCtrl[] m_BrainUIElements;

        #region Unity + Events

        private void Start() {
            TransformBGToMenuPos(null, false);
            SetUIElementsActiveState(m_MenuUIElements, true, false);
            SetUIElementsActiveState(m_BrainUIElements, false, false);
            SetUIElementsActiveState(m_GameplayUIElements, false, false);
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
#if UNITY_EDITOR
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                TransformBGToGameplayPos();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                TransformBGToMenuPos();
            }
        }
#endif

        private void OnInitGameplay() {
            SetUIElementsActiveState(m_MenuUIElements, false);  // Hide Menu UI
            SetUIElementsActiveState(m_BrainUIElements, true);  // Fade In Brain UI
            TransformBGToGameplayPos(() => StatesModule.GameStates.OnStartGameplay?.Invoke());  // Transform BG to Gamplay Pos
        }
        private void OnStartGameplay() {
            SetUIElementsActiveState(m_GameplayUIElements, true);   // Show Gameplay UI
        }
        private void OnGameOver() {
            SetUIElementsActiveState(m_GameplayUIElements, false);  // Hide Gameplay UI
            SetUIElementsActiveState(m_BrainUIElements, false);  // Fade Out Brain 
            TransformBGToMenuPos(() => SetUIElementsActiveState(m_MenuUIElements, true));   // Transform BG to Menu Pos
        }

        #endregion

        private void SetUIElementsActiveState(CanvasGroupTweenCtrl[] elements, bool activeState, bool animate = true) {
            if (elements == null || elements.Length.Equals(0)) return;

            foreach (var element in elements) {
                if (!animate) {
                    element.gameObject.SetActive(activeState);
                    continue;
                }

                if (activeState) {
                    element.SetAlpha(0);    // Make sure the alpha of the UI is 0 before the FadeIn
                    element.gameObject.SetActive(true);  // Make sure the UI element is active
                    element.AnimAlpha(0, 1, m_UIFadeTime)       // Then animate it in
                        .setEase(m_UIFadeEase);      
                } else {
                    element.gameObject.SetActive(true);  // Make sure the UI element is active before the FadeOut 
                    element.AnimAlpha(1, 0, m_UIFadeTime)   // Animate the UI element out, then disable it
                        .setEase(m_UIFadeEase)
                        .setOnComplete(() => element.gameObject.SetActive(false));
                }
            }
        }

        private int m_ToMenuPosTweenID = 0;
        private LTDescr TransformBGToMenuPos(System.Action callback = null, bool animate = true) {
            if (!m_HeadBGCtrl) return null;
            Vector2 newPos = new Vector2(150, -130);
            Vector2 newSize = new Vector2(-200, 0);
            Vector3 newRot = new Vector3(0, 0, 277);

#if UNITY_EDITOR
            if (!Application.isPlaying) animate = false;
#endif
            if (!animate) {
                m_HeadBGCtrl.RTransform.anchoredPosition = newPos;
                m_HeadBGCtrl.RTransform.sizeDelta = newSize;
                m_HeadBGCtrl.RTransform.localEulerAngles = newRot;
                return null;
            }

            LeanTween.cancel(m_ToMenuPosTweenID);

            float[] tweenTimes = new float[3];
            tweenTimes[0] = m_HeadBGCtrl.MoveAnim(m_HeadBGCtrl.RTransform.anchoredPosition, newPos, m_BGMoveAnimTime)
                .setEase(m_BGMoveEase).time;
            tweenTimes[1] = m_HeadBGCtrl.ScaleAnim(m_HeadBGCtrl.RTransform.sizeDelta, newSize, m_BGScaleAnimTime)
                .setEase(m_BGScaleEase).time;
            tweenTimes[2] = m_HeadBGCtrl.RotateAnim(new Vector3(0, 0, 360), newRot, m_BGRotateAnimTime)
                .setEase(m_BGRotateEase).time;

            if (callback == null) return null;
            float completeDelay = tweenTimes.Max(); // Gets the longest anim time from the 3 tweens and created a new empty LTDescr with a delay of that long
            LTDescr callbackTween = LeanTween.delayedCall(completeDelay, callback);
            m_ToMenuPosTweenID = callbackTween.uniqueId;
            return callbackTween;
        }

        private int m_ToGameplayPosTweenID = 0;
        private LTDescr TransformBGToGameplayPos(System.Action callback = null, bool animate = true) {
            if (!m_HeadBGCtrl) return null;
            Vector2 newPos = Vector2.zero;
            Vector2 newSize = Vector2.zero;
            Vector3 newRot = new Vector3(0, 0, 360);

#if UNITY_EDITOR
            if (!Application.isPlaying) animate = false;
#endif
            if (!animate) {
                m_HeadBGCtrl.RTransform.anchoredPosition = newPos;
                m_HeadBGCtrl.RTransform.sizeDelta = newSize;
                m_HeadBGCtrl.RTransform.localEulerAngles = newRot;
                return null;
            }
            LeanTween.cancel(m_ToGameplayPosTweenID);

            float[] tweenTimes = new float[3];
            tweenTimes[0] = m_HeadBGCtrl.MoveAnim(m_HeadBGCtrl.RTransform.anchoredPosition, newPos, m_BGMoveAnimTime)
                .setEase(m_BGMoveEase).time;
            tweenTimes[1] = m_HeadBGCtrl.ScaleAnim(m_HeadBGCtrl.RTransform.sizeDelta, newSize, m_BGScaleAnimTime)
                .setEase(m_BGScaleEase).time;
            tweenTimes[2] = m_HeadBGCtrl.RotateAnim(m_HeadBGCtrl.RTransform.localEulerAngles, newRot, m_BGRotateAnimTime)
                .setEase(m_BGRotateEase).time;

            if (callback == null) return null;
            float completeDelay = tweenTimes.Max(); // Gets the longest anim time from the 3 tweens and created a new empty LTDescr with a delay of that long
            LTDescr callbackTween = LeanTween.delayedCall(completeDelay, callback);
            m_ToGameplayPosTweenID = callbackTween.uniqueId;
            return callbackTween;
        }


        #region For In-Editor

        [ContextMenu("TransformBGToGameplayPos")]
        private void ToGameplay() {
            TransformBGToGameplayPos();
        }
        [ContextMenu("TransformBGToMenuPos")]
        private void ToMenu() {
            TransformBGToMenuPos();
        }

        #endregion
    }
}
