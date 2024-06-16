using System.Linq;
using UnityEngine;

public class SimpleTowerComponent : Tower, IShoot
{
    private EnemyComponent m_curTarget;
    void Update()
    {
        m_curCooldown -= Time.deltaTime;
        CatchTarget();
        
        if (CanShoot())
        {
        Shoot();
        }
    }
    
    public bool CanShoot()
    {
        if (!m_projectilePrefab || !m_spawnProjectilePoint)
        {
            Debug.LogError("The projectilePrefab, spawnProjectilePoint link is not installed or m_curTarget link is not");
            return false;
        }

        if (!m_curTarget)
        {
            return false;
        }
       
        if (!m_curTarget || m_curCooldown > 0)
        {
            return false;
        }
       
        return true;
    }

    public void CatchTarget()
    {
        if (m_targets.Count <= 0 )
        {
            return;
        }
       
        if (!m_curTarget && m_targets.Count > 0 || 
            m_curTarget && Vector3.Distance(transform.position, m_curTarget.transform.position) > m_range)
        {
            m_curTarget = m_targets.First();
        } 
    }
    
    public void Shoot()
    {
        var projectile = m_pool.GetFromPool(m_projectilePrefab, m_projectileParent);
        projectile.GetComponent<GuidedProjectileComponent>().Initialization(m_projectilePrefab, m_spawnProjectilePoint, m_curTarget.gameObject.transform, m_pool);
        projectile.gameObject.SetActive(true);
        m_curCooldown = m_cooldownSoot;
    }
}
