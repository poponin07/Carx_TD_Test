using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedProjectileComponent : Projectile, IMove
{
    
    public void Initialization(GameObject prefab, Transform spawnPoint, Transform target, DynamicPool.DynamicPool pool)
    {
        m_projectilePrefab = prefab;
        m_shootPoint = spawnPoint;
        transform.position = spawnPoint.position;
        m_target = target;
        m_pool = pool;
    }
    private void FixedUpdate()
    {
        Move();
        Rotation();
    }

    public void Move()
    {
        if (!m_target || !m_target.gameObject.activeSelf)
        {
            m_pool.ReturnToPool(m_projectilePrefab, gameObject);
            return;
        }
        transform.Translate(Vector3.forward * (m_speed * Time.deltaTime));
    }
    
    public void Rotation()
    {
        transform.LookAt(m_target);
    }
    
    
    
    
}
