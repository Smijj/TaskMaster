using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.BarrierModule
{
    public class BarrierCtrl : MonoBehaviour
    {
        [SerializeField] private RectTransform m_PivotPoint;
        [SerializeField] private RectTransform m_Barrier;
        [SerializeField] private float m_RotationSpeed = 20;
        [SerializeField] private float m_RotationRadius = 155;
        
        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;

            // Move Barrier to its starting pos
            m_Barrier.localPosition = new Vector3(0, -m_RotationRadius, 0);
        }

        private void Update() {

            // Keep the shield clamped to below the pivot point (i.e. 180 degrees of freedom)
            if (m_Barrier.localPosition.y <= 0) {
                float mouseInputDirection = Input.GetAxis("Mouse X");
                m_Barrier.RotateAround(m_PivotPoint.position, Vector3.forward, mouseInputDirection * m_RotationSpeed * Time.deltaTime);
            } else {
                // Move the shield back into the clamp range
                if (m_Barrier.localPosition.x < 0) {
                    m_Barrier.localPosition = new Vector3(-m_RotationRadius, 0, 0);
                    m_Barrier.localEulerAngles = new Vector3(0, 0, -90);
                } else {
                    m_Barrier.localPosition = new Vector3(m_RotationRadius, 0, 0);
                    m_Barrier.localEulerAngles = new Vector3(0, 0, 90);
                }
            }

        }
    }
}
