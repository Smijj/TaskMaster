using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.BarrierModule
{
    public class BarrierCtrl : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_RotationSpeed = 20;
        [SerializeField] private float m_RotationRadius = 155;
        
        [Header("References")]
        [SerializeField] private RectTransform m_PivotPoint;
        [SerializeField] private RectTransform m_Barrier;

        [Header("Debug")]
        [ReadOnly, SerializeField] private float currentMouseXPosInPixels = Screen.width / 2;


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
        private void Start() {
            

            
        }
        private void OnInitGameplay() {
            m_Barrier.localPosition = new Vector3(0, -m_RotationRadius, 0); // Move Barrier to its starting pos
        }
        private void OnStartGameplay() {
            Cursor.lockState = CursorLockMode.Locked;   // Lock the mouse when the gameplay starts
        }
        private void OnGameOver() {
            Cursor.lockState = CursorLockMode.None;   // Unlock the mouse when the game is over
        }

        private void Update() {
            HandleBarrierMovement();
        }

        //private Vector3 m_SmoothVelocityRef = Vector3.zero;
        private void HandleBarrierMovement() {
            if (!StatesModule.GameStates.IsGamePlaying) return;

            // Get mouse X Input delta and add it to the currentMouseXPos
            float mouseXDelta = Input.GetAxis("Mouse X");
            currentMouseXPosInPixels += mouseXDelta * m_RotationSpeed;

            // To not allow the currentMouseXPosInPixels to leave the size of the screen.
            if (currentMouseXPosInPixels < 0) currentMouseXPosInPixels = 0;
            else if (currentMouseXPosInPixels > Screen.width) currentMouseXPosInPixels = Screen.width;

            // Get mouse X pos on the screen as a Percentage.
            float mouseXPosPercentage = currentMouseXPosInPixels / Screen.width;
            
            (Vector2, float) barrierTransformInfo = GetPointAndRotationAlongCircleCircumference(m_PivotPoint, m_RotationRadius, mouseXPosPercentage, 1.1f, 1.9f);
            float rotationInDeg = (Mathf.Rad2Deg * barrierTransformInfo.Item2) + 90;
            //Vector3 newBarrierPos = Vector3.SmoothDamp(m_Barrier.transform.localPosition, barrierTransformInfo.Item1, ref m_SmoothVelocityRef, m_SmoothTime);
            Vector3 newBarrierPos = barrierTransformInfo.Item1;
            m_Barrier.transform.localPosition = newBarrierPos;
            m_Barrier.transform.eulerAngles = new Vector3(0, 0, rotationInDeg);
        }

        private (Vector3, float) GetPointAndRotationAlongCircleCircumference(RectTransform circleOrigin, float radius, float mouseXPosPercentage, float circumferenceRangeMin = 1f, float circumferenceRangeMax = 2f) {

            // Calculate the 'x' 'y' of a point on the circumference of a circle that has 'r' radius, 'a' angle (in radians 0..2PI) and is at the position 'cx' 'cy'
            // x = cx + r * cos(a)
            // y = cy + r * sin(a)

            Vector3 point = new Vector3();
            float radianAngle = Mathf.PI * Mathf.Lerp(circumferenceRangeMin, circumferenceRangeMax, mouseXPosPercentage);
            point.x = circleOrigin.position.x + radius * Mathf.Cos(radianAngle);
            point.y = circleOrigin.position.y + radius * Mathf.Sin(radianAngle);
            return (point, radianAngle);
        }
    }
}
