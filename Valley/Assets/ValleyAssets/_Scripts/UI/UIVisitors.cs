using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisitors : MonoBehaviour
{
    private void Start()
    {
        VisitorManager.OnVisitorsUpdate += UpdateVisitors;
    }

    public void UpdateVisitors(int i)
    {
        UIManager.UpdateVisitors(i);
    }

    public void OnDisable()
    {
        VisitorManager.OnVisitorsUpdate -= UpdateVisitors;
    }
}
