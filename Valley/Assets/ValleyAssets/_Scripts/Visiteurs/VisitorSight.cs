using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSight : MonoBehaviour
{
    // The linked visitor.
    [SerializeField] private VisitorAgentBehave visitor;

    /// <summary>
    /// Detect when the visitor see an InterestPoint.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        InterestPoint point = other.GetComponent<InterestPoint>(); // Voir pour ne pas utiliser de GetComponent
        if (point != null)
        {
            visitor.AskInteraction(point);
        }
    }
}
