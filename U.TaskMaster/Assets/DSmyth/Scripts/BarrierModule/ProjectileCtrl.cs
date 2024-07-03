using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.BarrierModule
{
    public class ProjectileCtrl : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D m_Rigid;

        private void Reset() {
            if (!m_Rigid) m_Rigid = GetComponent<Rigidbody2D>();
        }
        private void Awake() {
            if (!m_Rigid) m_Rigid = GetComponent<Rigidbody2D>();
        }
        public void ShootProjectile(Vector2 force, float lifetime) {
            if (m_Rigid) m_Rigid.AddForce(force, ForceMode2D.Impulse);  // Add force in direction
            Invoke(nameof(DestroyProjectile), lifetime);                // Destroy projectile after lifetime
        }
        private void DestroyProjectile() {
            Destroy(gameObject);
        }
    }
}
