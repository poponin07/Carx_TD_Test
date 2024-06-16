using UnityEngine;

public class PathPointComponent : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position,.5f);
    }
}
