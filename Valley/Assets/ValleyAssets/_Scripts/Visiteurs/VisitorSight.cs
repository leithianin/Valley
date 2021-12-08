using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorSight : MonoBehaviour
{
    [SerializeField] private VisitorAgentBehave visitor;

    private void OnTriggerEnter(Collider other)
    {

        InterestPoint point = other.GetComponent<InterestPoint>(); // Voir pour ne pas utiliser de GetComponent
        if (point != null)
        {
            visitor.AskInteraction(point);
        }
    }
}
