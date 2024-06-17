using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enemy;

public abstract class Tower : MonoBehaviour, IShoot
{
    [Header("Tower Settings")]
    [SerializeField] protected float m_cooldownSoot;
    [SerializeField] protected float m_range;
    
    [Space][Header("References")]
    [SerializeField] protected GameObject m_projectilePrefab;
    [SerializeField] protected Transform m_spawnProjectilePoint;
    [SerializeField] protected Transform m_projectileParent;
    [SerializeField] protected List<EnemyComponent> m_targets = new List<EnemyComponent>();
    [SerializeField] protected DynamicPool.DynamicPool m_pool;
    
    protected EnemyComponent m_curTarget;
    protected float m_curCooldown;
    private SphereCollider m_rangeCollider;
    private List<EnemyComponent> m_subscriptions = new List<EnemyComponent>();

    public void Start()
    {
        m_rangeCollider = GetComponent<SphereCollider>();
        m_rangeCollider.radius = m_range;
        m_pool.CreatePool(m_projectilePrefab, 2, m_projectileParent);
    }
    
    public virtual bool CanShoot()
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

    public virtual void CatchTarget()
    {
        if (m_targets.Count <= 0 )
        {
            return;
        }
       
        if (!m_curTarget && m_targets.Count > 0 || 
            m_curTarget && Vector3.Distance(transform.position, m_curTarget.transform.position) > m_range ||
            !m_curTarget.gameObject.activeSelf)
        {
            m_curTarget = m_targets.First();
        } 
    }

    public virtual void Shoot()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyComponent>();
        if (enemy)
        {
            m_targets.Add(enemy);
            m_subscriptions.Add(enemy);
            enemy.onEnemyDie += RemoveEnemyFromTargetsByAction;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<EnemyComponent>();
        if (enemy)
        {
            m_targets.Remove(enemy);
            m_subscriptions.Remove(enemy);
            enemy.onEnemyDie -= RemoveEnemyFromTargetsByAction;
        }
    }

    private void RemoveEnemyFromTargetsByAction(EnemyComponent enemy)
    {
        if (m_targets.Contains(enemy))
        {
            m_targets.Remove(enemy);
        }
    }
    
    private void OnDisable()
    {
        foreach (var enemy in m_subscriptions)
        {
            if (enemy)
            {
                enemy.onEnemyDie -= RemoveEnemyFromTargetsByAction;
            }
        }
    }
}
