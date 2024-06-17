using UnityEngine;

public class CannonProjectileComponent : Projectile, IMove
{

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
