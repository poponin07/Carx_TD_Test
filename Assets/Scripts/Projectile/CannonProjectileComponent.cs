using UnityEngine;

public class CannonProjectileComponent : Projectile, IMove
{

    public void Initialization(GameObject prefab, Transform spawnPoint, Transform target, DynamicPool.DynamicPool pool)
    {
        m_projectilePrefab = prefab;
        m_shootPoint = spawnPoint;
        transform.position = spawnPoint.position;
        m_target = target;
        m_pool = pool;
    }
    public void LaunchingProjectile()
    {
        Rotation();
        Move();
    }
    
    public float GetSpeed()
    {
        return m_speed;
    }
    
    public void Move()
    {
        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * m_speed; 
    }

    public void Rotation()
    {
        gameObject.transform.rotation = m_projectilePrefab.transform.rotation;
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, m_shootPoint.forward);
        gameObject.transform.rotation = rotation;
    }
}
