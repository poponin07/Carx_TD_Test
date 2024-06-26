﻿using UnityEngine;


public class ConnonTowerComponent : Tower, ICannonAiming
{
    [SerializeField] private float trackingSpeed;
    private Vector3 targetPoint;
    private float m_projectileSpeed;

    private void Start()
    {
        base.Start();
        m_projectileSpeed = m_projectilePrefab.GetComponent<CannonProjectileComponent>().GetSpeed();
        m_curCooldown = m_cooldownSoot;
    }
    private void Update()
    {
        m_curCooldown -= Time.deltaTime;
        CatchTarget();
        
        if (CanShoot() && IsTowerAimedAtTarget(targetPoint))
        {
            Shoot();
        }

        if (m_curTarget)
        {
            targetPoint = RredictionCollisionPoint(m_curTarget.gameObject);
            float shootAngle = CalculationShootAngle(targetPoint);
           RotateTurretTowards(targetPoint, shootAngle);
        }
    }
    
    
    public override void Shoot()
        {
            var projectile = m_pool.GetFromPool(m_projectilePrefab, m_projectileParent);
            CannonProjectileComponent projComp = projectile.GetComponent<CannonProjectileComponent>();
            
            projComp.Initialization(m_projectilePrefab, m_spawnProjectilePoint, 
                m_curTarget.gameObject.transform, m_pool);
            projComp.LaunchingProjectile();
            projectile.SetActive(true);
            m_curCooldown = m_cooldownSoot;
        }
    
    public Vector3 RredictionCollisionPoint(GameObject target)
        {
            if (!target)
            {
                return Vector3.zero;
            }

            Vector3 targetPosition = target.transform.position;
            var targetVelocity = target.GetComponent<Rigidbody>().velocity;
            Vector3 displacement = targetPosition - transform.position;
            float targetMoveAngle = Vector3.Angle(displacement, targetVelocity) * Mathf.Deg2Rad;

            if (targetMoveAngle == 0)
            {
                return targetPosition;
            }

            float shooterTargetDist = displacement.magnitude;
            float targetVelocityMagnitude = targetVelocity.magnitude;

            float a = m_projectileSpeed * m_projectileSpeed - targetVelocityMagnitude * targetVelocityMagnitude;
            float b = 2 * shooterTargetDist * targetVelocityMagnitude * Mathf.Cos(targetMoveAngle);
            float c = -shooterTargetDist * shooterTargetDist;

            float discriminant = b * b - 4 * a * c;


            if (discriminant < 0)
            {
                Debug.LogError("The discriminant is negative. There is no solution");
                return Vector3.zero;
            }

            float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

            float timeToCollision = Mathf.Max(Mathf.Min(t1, t2), Mathf.Max(t1, t2));

            if (timeToCollision < 0)
            {
                Debug.LogError("The timeToCollision is negative");
                return Vector3.zero;
            }

            Vector3 aimPoint = targetPosition + targetVelocity * timeToCollision;
            return aimPoint;
        }
    public float CalculationShootAngle(Vector3 targetPosition)
        {
            float g = Physics.gravity.magnitude;
            float offsetY = targetPosition.y;
            float distanceX = targetPosition.magnitude;
            float speedSquared = m_projectileSpeed * m_projectileSpeed;

            float underSqrt = (speedSquared * speedSquared) -
                              g * (g * distanceX * distanceX + 2 * offsetY * speedSquared);

            if (underSqrt < 0)
            {
                Debug.LogError("The root underSqrt is negative");
                return -1f;
            }

            float sqrt = Mathf.Sqrt(underSqrt);

            float highAngle = Mathf.Atan2(speedSquared + sqrt, g * distanceX);
            float lowAngle = Mathf.Atan2(speedSquared - sqrt, g * distanceX);

            return Mathf.Min(highAngle, lowAngle) * Mathf.Rad2Deg;
        }

    private void RotateTurretTowards(Vector3 targetPosition, float shootAngle)
    {
        Vector3 directionToTarget = targetPosition - gameObject.transform.position;
        directionToTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        var rotationY = Quaternion.Euler(shootAngle, transform.localEulerAngles.y, 0f);

        transform.localRotation =
            Quaternion.RotateTowards(transform.localRotation, rotationY, trackingSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, targetRotation,
            trackingSpeed * Time.deltaTime);
    }
    
    public bool IsTowerAimedAtTarget(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        if (Vector3.Dot(transform.forward, directionToTarget) > .99)
        {
            return true;
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPoint,1);
    }
    
}