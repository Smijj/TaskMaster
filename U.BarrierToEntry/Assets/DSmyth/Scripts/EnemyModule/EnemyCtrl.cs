using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.EnemyModule
{
    public class EnemyCtrl : MonoBehaviour
    {
        [SerializeField] private float m_TravelTime = 4f;
        [SerializeField] private float m_TravelTimeCounter = 0;
        private RectTransform m_Target;
        private Vector3 m_StartPos;

        private void Start() {
            m_TravelTimeCounter = 0;
        }

        private void Update() {
            HandleEnemyMovement();
        }

        public void SetTarget(RectTransform target) {
            m_Target = target;
            m_StartPos = transform.position;
        }

        private void HandleEnemyMovement() {
            if (!m_Target) return;

            m_TravelTimeCounter += Time.deltaTime;
            if (m_TravelTimeCounter >= m_TravelTime) Destroy(gameObject);   // If something goes wrong and the enemy doesnt hit any of the colliders that should destroy it, clean it up at the end of its movement path.

            transform.position = Vector3.Lerp(m_StartPos, m_Target.position, m_TravelTimeCounter / m_TravelTime);
        }

        private void OnTriggerEnter2D(Collider2D collision) {

            Debug.Log("Trigger Entered " + collision.gameObject.name);

            if (collision.CompareTag("Barrier")) {
                Destroy(gameObject);
            }
            if (collision.CompareTag("ProtectionArea")) {
                // Reduce player health
                Destroy(gameObject);
            }
        }
    }
}
