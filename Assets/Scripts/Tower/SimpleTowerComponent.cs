using UnityEngine;

public class SimpleTowerComponent : Tower, IShoot
{
    void Update()
    {
        m_curCooldown -= Time.deltaTime;
        CatchTarget();
        
        if (CanShoot())
        {
        Shoot();
        }
    }
    
    public override void Shoot()
    {
        var projectile = m_pool.GetFromPool(m_projectilePrefab, m_projectileParent);
        projectile.GetComponent<GuidedProjectileComponent>().Initialization(m_projectilePrefab, m_spawnProjectilePoint, m_curTarget.gameObject.transform, m_pool);
        projectile.gameObject.SetActive(true);
        m_curCooldown = m_cooldownSoot;
    }
}
