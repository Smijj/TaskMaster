using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace DSmyth.EnemyModule
{
    public class LogoEnemyCtrl : EnemyCtrl
    {
        [Header("Video Enemy Settings")]
        [SerializeField] private float m_FragmentLifetime = 1.5f;
        [SerializeField] private Material m_DeathSpriteMaterial;

        [Header("References")]
        [SerializeField] private Explodable m_Explodable;

        private void Reset() {
            if (!m_Explodable) m_Explodable = GetComponent<Explodable>();
        }

        public override void Awake() {
            base.Awake();

            if (!m_Explodable) m_Explodable = GetComponent<Explodable>();

            if (m_Explodable && m_Explodable.fragments.Count > 0) {
                foreach (var fragment in m_Explodable.fragments) {
                    MeshRenderer MeshRen = fragment.GetComponent<MeshRenderer>();
                    if (MeshRen && m_DeathSpriteMaterial) MeshRen.material = m_DeathSpriteMaterial;   // Set Enemy Death Sprite Material
                }
            }
        }

        public override void Die() {
            m_IsDead = true;    // Stops enemy from moving
            if (m_Collider) m_Collider.enabled = false;

            if (m_Explodable) m_Explodable.explode(10f);

            // Wait a few seconds then destroy the enemy object
            Invoke("DestroyThis", m_FragmentLifetime);
        }

    }
}
