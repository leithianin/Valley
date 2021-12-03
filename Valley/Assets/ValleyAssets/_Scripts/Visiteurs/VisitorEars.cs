using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorEars : MonoBehaviour
{
    [SerializeField] private VisitorAgentBehave visitor;

    private void OnTriggerEnter(Collider other)
    {
        VisitorAgentBehave otherVisitor = other.GetComponent<VisitorAgentBehave>(); // Voir pour ne pas utiliser de GetComponent
        Debug.Log("TestTrigger");
        if (otherVisitor != null && otherVisitor != visitor)
        {
            Debug.Log("Detect other visitor");
            visitor.CrossVisitor();
        }
    }
}
