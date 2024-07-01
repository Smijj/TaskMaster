using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DSmyth.EnemyModule
{
    public class EnemyCtrl : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] private float m_TravelTime = 4f;
        [SerializeField] private int m_EnemyDamage = 5;
        private float m_TravelTimeCounter = 0;
        [ReadOnly, SerializeField] private bool m_Initialized = false;
        
        [Header("References")]
        [SerializeField] protected Collider2D m_Collider;
        [SerializeField] protected ParticleSystem m_DistractionSucessfullVFX;

        protected bool m_IsDead = false;
        private RectTransform m_Target;
        private Vector2 m_StartPos;


        #region Unity + Events

        private void Reset() {
            if (!m_Collider) m_Collider = GetComponent<Collider2D>();
        }
        public virtual void Awake() {
            m_TravelTimeCounter = 0;
            if (!m_Collider) m_Collider = GetComponent<Collider2D>();
        }
        private void OnEnable() {
            StatesModule.GameStates.OnGameOver += OnGameOver;
        }
        private void OnDisable() {
            StatesModule.GameStates.OnGameOver -= OnGameOver;
        }
        private void Update() {
            HandleEnemyMovement();
        }

        private void OnGameOver() {
            Die();  // Get rid of any remaining enemies when the game is over
        }

        #endregion


        public void SetTarget(RectTransform target, Vector2 spawnPos) {
            transform.position = spawnPos;
            m_Target = target;
            m_StartPos = spawnPos;
            m_Initialized = true;
        }

        private void HandleEnemyMovement() {
            if (!m_Target || m_IsDead || !m_Initialized) return;

            m_TravelTimeCounter += Time.deltaTime;
            if (m_TravelTimeCounter >= m_TravelTime) DestroyThis();   // If something goes wrong and the enemy doesnt hit any of the colliders that should destroy it, clean it up at the end of its movement path.

            transform.position = Vector2.Lerp(m_StartPos, m_Target.position, m_TravelTimeCounter / m_TravelTime);
        }
        
        public virtual void Die() {
            m_IsDead = true;    // Stops enemy from moving
            DestroyThis();
        }
        protected void DestroyThis() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (m_IsDead) return;

            if (collision.CompareTag("Barrier")) {
                StatesModule.EnemyStates.OnDistractionBlocked?.Invoke();  
                Die();
            }
            else if (collision.CompareTag("Projectile")) {
                StatesModule.EnemyStates.OnDistractionDestroyed?.Invoke();  // Give player points for actively shooting down a distraction
                Die();
            }
            else if (collision.CompareTag("ProtectionArea")) {
                // Reduce player health
                StatesModule.EnemyStates.OnDistractionSucessful?.Invoke(m_EnemyDamage);
                // Instantiate VFX
                if (m_DistractionSucessfullVFX) Instantiate(m_DistractionSucessfullVFX, transform.position, transform.rotation, transform.parent.transform);
                //Die();
                m_IsDead = true;
                DestroyThis();
            }
        }
    }
}
