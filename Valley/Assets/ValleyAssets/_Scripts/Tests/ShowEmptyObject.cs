using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowEmptyObject : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
