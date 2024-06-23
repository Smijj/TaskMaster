using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DSmyth.EnemyModule
{
    public class EnemyCtrl : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_TravelTime = 4f;
        [ReadOnly, SerializeField] private float m_TravelTimeCounter = 0;
        [ReadOnly, SerializeField] private bool m_Initialized = false;
        [SerializeField] private VideoClip m_VideoToPlay;
        
        [Header("References")]
        [SerializeField] private Collider2D m_Collider;
        [SerializeField] private VideoPlayer m_VPlayer;
        [SerializeField] private Explodable m_Explodable;

        [Header("Debug")]
        [SerializeField] private List<VideoPlayer> m_FragmentVPlayers = new List<VideoPlayer>();
        
        private RectTransform m_Target;
        private Vector3 m_StartPos;
        private bool m_IsDead = false;


        private void Awake() {
            m_TravelTimeCounter = 0;
            if (!m_Collider) m_Collider = GetComponent<Collider2D>();
            if (!m_VPlayer) m_VPlayer = GetComponentInChildren<VideoPlayer>();
            if (!m_Explodable) m_Explodable = GetComponentInChildren<Explodable>();

            if (m_VPlayer && m_VideoToPlay) m_VPlayer.clip = m_VideoToPlay;
            if (m_Explodable && m_Explodable.fragments.Count > 0) {
                foreach (var fragment in m_Explodable.fragments) {
                    VideoPlayer player = fragment.GetComponent<VideoPlayer>();
                    if (m_VideoToPlay) player.clip = m_VideoToPlay;
                    m_FragmentVPlayers.Add(player);
                }
            }
            //foreach (var videoPlayer in m_FragmentVPlayers) {
            //    if (videoPlayer.gameObject.activeSelf) videoPlayer.Prepare();
            //}
        }

        private void Update() {
            HandleEnemyMovement();
        }

        public void SetTarget(RectTransform target) {
            m_Target = target;
            m_StartPos = transform.position;
            m_Initialized = true;
        }

        private void HandleEnemyMovement() {
            if (!m_Target || m_IsDead || !m_Initialized) return;

            m_TravelTimeCounter += Time.deltaTime;
            if (m_TravelTimeCounter >= m_TravelTime) Die();   // If something goes wrong and the enemy doesnt hit any of the colliders that should destroy it, clean it up at the end of its movement path.

            transform.position = Vector3.Lerp(m_StartPos, m_Target.position, m_TravelTimeCounter / m_TravelTime);
        }

        private void HandleDeath() {
            m_IsDead = true;    // Stops enemy from moving

            if (m_Explodable) {
                Shatter();
            } else {
                Die();
            }
        }
        private void Shatter() {
            if (m_Collider) m_Collider.enabled = false;

            m_Explodable.explode(10f);
            foreach (var player in m_FragmentVPlayers) {
                player.Play();
                player.frame = m_VPlayer.frame;
                player.Pause();
            }

            // Wait a few seconds then destroy the enemy object
            Invoke("Die", 3f);
        }
        private void Die() {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision) {

            Debug.Log("Trigger Entered " + collision.gameObject.name);

            if (collision.CompareTag("Barrier")) {
                HandleDeath();
            }
            if (collision.CompareTag("ProtectionArea")) {
                // Reduce player health
                HandleDeath();
            }
        }
    }
}
