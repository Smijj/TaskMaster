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



        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;

            // Move Barrier to its starting pos
            m_Barrier.localPosition = new Vector3(0, -m_RotationRadius, 0);
        }

        private void Update() {

            HandleBarrierMovement();

            /* Old Code
            //Keep the shield clamped to below the pivot point(i.e. 180 degrees of freedom)
            if (m_Barrier.localPosition.y <= 0) {
                float mouseInputDirection = Input.GetAxis("Mouse X"); // Gets the number of pixels the mouse has moved in the X axis since last frame (doesnt need to be multiplied by Time.deltaTime
                m_Barrier.RotateAround(m_PivotPoint.position, Vector3.forward, mouseInputDirection * m_RotationSpeed);
            }
            else {
                // Move the shield back into the clamp range
                if (m_Barrier.localPosition.x < 0) {
                    m_Barrier.localPosition = new Vector3(-m_RotationRadius, 0, 0);
                    m_Barrier.localEulerAngles = new Vector3(0, 0, -90);
                }
                else {
                    m_Barrier.localPosition = new Vector3(m_RotationRadius, 0, 0);
                    m_Barrier.localEulerAngles = new Vector3(0, 0, 90);
                }
            }*/

        }

        //private Vector3 m_SmoothVelocityRef = Vector3.zero;
        private void HandleBarrierMovement() {

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
