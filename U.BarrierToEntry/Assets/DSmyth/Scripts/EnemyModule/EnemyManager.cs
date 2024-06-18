using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSmyth.EnemyModule
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform m_EnemyParent;
        [SerializeField] private EnemyCtrl m_EnemyPrefab;
        [SerializeField] private RectTransform m_CenterPoint;
        
        [Header("Enemy Settings")]
        [SerializeField] private float m_EnemySpawnTime = 2f;
        private float m_EnemySpawnTimeCounter = 0;
        
        [Header("Spawn Position Settings")]
        [SerializeField] private float m_EnemySpawnRadius = 10f;
        [SerializeField] private float m_SpawnCircumferenceRangeMin = 1.3f;
        [SerializeField] private float m_SpawnCircumferenceRangeMax = 1.7f;


        //private List<EnemyCtrl> m_ActiveEnemies = new List<EnemyCtrl>();

        [Header("Debug")]
        [SerializeField] private bool m_DrawGizmos = true;

        private void Update() {
            // Spawn Enemies every few seconds
            if (m_EnemySpawnTimeCounter > m_EnemySpawnTime) {
                SpawnEnemy();
                m_EnemySpawnTimeCounter = 0;
            }
            m_EnemySpawnTimeCounter += Time.deltaTime;
        }


        [ContextMenu("SpawnEnemy")]
        private void SpawnEnemy() {
            Vector3 spawnPos = GetRandomPointAlongCircleCircumference(m_CenterPoint, m_EnemySpawnRadius, m_SpawnCircumferenceRangeMin, m_SpawnCircumferenceRangeMax);
            EnemyCtrl enemyCtrl = Instantiate(m_EnemyPrefab, m_EnemyParent);
            enemyCtrl.transform.position = new Vector3(spawnPos.x, spawnPos.y, m_EnemyParent.position.z);
            enemyCtrl.SetTarget(m_CenterPoint);

            //m_ActiveEnemies.Add(enemyCtrl);
        }

        private Vector3 GetRandomPointAlongCircleCircumference(RectTransform circleOrigin, float radius, float circumferenceRangeMin = 0f, float circumferenceRangeMax = 2f) {

            // Calculate the 'x' 'y' of a point on the circumference of a circle that has 'r' radius, 'a' angle (in radians 0..2PI) and is at the position 'cx' 'cy'
            // x = cx + r * cos(a)
            // y = cy + r * sin(a)

            Vector3 randomPoint = new Vector3();
            float randomRadianPoint = Mathf.PI * Random.Range(circumferenceRangeMin, circumferenceRangeMax);
            randomPoint.x = circleOrigin.position.x + radius * Mathf.Cos(randomRadianPoint);
            randomPoint.y = circleOrigin.position.y + radius * Mathf.Sin(randomRadianPoint);
            return randomPoint;
        }


        private void OnDrawGizmos() {
            if (!m_DrawGizmos) return;
            Gizmos.color = Color.red;

            if (m_CenterPoint) Gizmos.DrawWireSphere(m_CenterPoint.position, m_EnemySpawnRadius);
        }

    }
}
