using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShoot
{
    bool CanShoot();
    void CatchTarget();
    void Shoot();
}
