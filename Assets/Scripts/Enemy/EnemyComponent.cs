using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyComponent : MonoBehaviour, ITakeDamage, IMove
    {
        [Header("Enemy Settings")] [SerializeField]
        private int m_maxHP;

        [SerializeField] private int m_damage;
        [SerializeField] private GameObject m_enemyPrefab;
        [SerializeField] private List<Transform> moveTargets;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float m_speed;
    
        private int m_currentHP;
        private Transform curTarget;
        private int pointIndex;
        private DynamicPool.DynamicPool m_pool;
        private Rigidbody m_rb;
    
        public Action<EnemyComponent> onEnemyDie;

        private void Start()
        {
            m_rb = gameObject.GetComponent<Rigidbody>();
        }

        public void Initialization(List<Transform> target, DynamicPool.DynamicPool pool, GameObject prefab, Vector3 spawnPoint)
        {
            m_currentHP = m_maxHP;
            moveTargets = target;
            curTarget = moveTargets[0];
            m_pool = pool;
            m_enemyPrefab = prefab;
        }

        private void FixedUpdate()
        {
            Move();
            Rotation();
        }

        public void Move()
        {
            if (!curTarget) return;
		
            if (Vector3.Distance(transform.position, curTarget.position) <= 1)
            {
                pointIndex++;
                curTarget = moveTargets[pointIndex];
            }
            Rotation();
            m_rb.velocity = Vector3.right * (m_speed * Time.deltaTime);
        }
    
        public void Rotation()
        {
            Vector3 directionToTarget = curTarget.position - transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        public void TakeDamage(int damage)
        {
            m_currentHP -= damage;

            if (m_currentHP <= 0)
            {
                onEnemyDie?.Invoke(this);
                m_pool.ReturnToPool(m_enemyPrefab, gameObject);
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Base")
            {
                m_pool.ReturnToPool(m_enemyPrefab, gameObject);
            }
        }
    }
}