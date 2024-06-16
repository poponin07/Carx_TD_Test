using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICannonAiming
{
    bool IsTowerAimedAtTarget(Vector3 targetPosition);
    Vector3 RredictionCollisionPoint(GameObject target);
    float CalculationShootAngle(Vector3 targetPosition);
}
