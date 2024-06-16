using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileStruct 
{
    [Header("Projectile Settings")] 
    [SerializeField][Range(20,50)] public float m_speed;
    public int m_damage;
    public GameObject m_projectilePrefab;
    public Transform m_shootPoint;
    public DynamicPool.DynamicPool m_pool;
    public Transform m_target;
}
