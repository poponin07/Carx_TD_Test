using System;
using UnityEngine;

public class GuidedProjectileComponent : Projectile, IMove
{
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
