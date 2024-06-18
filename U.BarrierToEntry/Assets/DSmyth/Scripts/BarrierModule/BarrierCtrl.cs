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


        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            
        }

        private void Update() {
            float mouseInputDirection = Input.GetAxis("Mouse X");
            m_Barrier.RotateAround(m_PivotPoint.position, Vector3.forward, mouseInputDirection * m_RotationSpeed * Time.deltaTime);
        }
    }
}
