using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorEars : MonoBehaviour
{
    // The linked visitor.
    [SerializeField] private VisitorAgentBehave visitor;

    /// <summary>
    /// Used to check if a visitor see an other visitor.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        VisitorAgentBehave otherVisitor = other.GetComponent<VisitorAgentBehave>(); // Voir pour ne pas utiliser de GetComponent
        if (otherVisitor != null && otherVisitor != visitor)
        {
            visitor.CrossVisitor();
        }
    }
}
