using UnityEngine;
using System.Collections.Generic;

public class SpawnComponent : MonoBehaviour
{
	[Header("Spawn Settings")]
	[SerializeField] private Transform m_spawnPoint;
	[SerializeField] private GameObject m_enemyPrefab;
	[SerializeField] private float m_spawnInterval;
	[SerializeField] private List<Transform> m_targetsToMove;
	private float m_curCooldown;
	private float m_lastTimeSpawn;

	[Space] [Header("References")] 
	[SerializeField] private DynamicPool.DynamicPool m_pool;
	[SerializeField] private Transform m_enemyParent;

	private void Start()
	{
		m_pool.CreatePool(m_enemyPrefab, 3, m_enemyParent);
	}

	void Update ()
	{
		m_curCooldown -= Time.deltaTime;
		if (m_curCooldown <= 0) 
		{
			var enemy = m_pool.GetFromPool(m_enemyPrefab, m_enemyParent);
			enemy.transform.position = m_spawnPoint.position;
			enemy.GetComponent<EnemyComponent>().Initialization(m_targetsToMove, m_pool, m_enemyPrefab);
			enemy.SetActive(true);
			m_curCooldown = m_spawnInterval;
		}
	}
	
	
}
