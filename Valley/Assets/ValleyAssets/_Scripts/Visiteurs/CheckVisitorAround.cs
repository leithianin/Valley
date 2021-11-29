using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckVisitorAround : MonoBehaviour
{
    private List<VisitorAgentBehave> closeVisitor = new List<VisitorAgentBehave>();

    [SerializeField] private VisitorAgentBehave selfAgent;

    private void OnTriggerEnter(Collider other)
    {
        VisitorAgentBehave visitor = other.GetComponent<VisitorAgentBehave>();
        if(visitor != null && visitor != selfAgent && !closeVisitor.Contains(visitor))
        {
            Debug.Log(visitor.Agent.avoidancePriority + " > " + selfAgent.Agent.avoidancePriority);
            if (visitor.Agent.avoidancePriority > selfAgent.Agent.avoidancePriority)
            {
                visitor.SetObstacle(true);
                closeVisitor.Add(visitor);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        VisitorAgentBehave visitor = other.GetComponent<VisitorAgentBehave>();
        if (visitor != null && closeVisitor.Contains(visitor))
        {
            closeVisitor.Remove(visitor);
            if (closeVisitor.Count <= 0)
            {
                visitor.SetObstacle(false);
            }
        }
    }
}
