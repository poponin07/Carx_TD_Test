using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")] 
    [SerializeField] [Range(20,50)] protected float m_speed;
    [SerializeField] protected int m_damage;
    protected GameObject m_projectilePrefab;
    protected Transform m_shootPoint;
    protected DynamicPool.DynamicPool m_pool;
    protected Transform m_target;
    
    public void Initialization(GameObject prefab, Transform spawnPoint, Transform target, DynamicPool.DynamicPool pool)
    {
        m_projectilePrefab = prefab;
        m_shootPoint = spawnPoint;
        transform.position = spawnPoint.position;
        m_target = target;
        m_pool = pool;
    }
  
    void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyComponent>();
        
        if (enemy)
        {
            enemy.TakeDamage(m_damage);
            m_pool.ReturnToPool(m_projectilePrefab, gameObject);
        }

        else
        {
            m_pool.ReturnToPool(m_projectilePrefab, gameObject);
        }
    }
}
