using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyManager : MonoBehaviour
{
    private static ValleyManager instance;

    [SerializeField] private float currentAttractivity;

    public static float RealAttractivity => instance.currentAttractivity;
    public static int AttractivityLevel => Mathf.FloorToInt(instance.currentAttractivity);


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(UpdateAttractivityDelay());
    }

    IEnumerator UpdateAttractivityDelay()
    {
        yield return new WaitForSeconds(5f);
        UpdateAttractivity();
        StartCoroutine(UpdateAttractivityDelay());
    }

    public static void UpdateAttractivity()
    {
        List<VisitorAgentBehave> visitors = VisitorManager.GetVisitors;
        List<ValleyArea> areas = ValleyAreaManager.GetAreas;

        float visitorScore = 0;
        if (visitors.Count > 0)
        {
            for (int i = 0; i < visitors.Count; i++)
            {
                visitorScore += visitors[i].Data.satisfactionScore;
            }

            visitorScore = visitorScore / visitors.Count;
        }

        float areasScore = 0;

        if (areas.Count > 0)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                areasScore += areas[i].NoiseScore / 20;
            }

            areasScore = areasScore / areas.Count;
        }
        instance.currentAttractivity = ((visitorScore + (1-areasScore)) / 2) * 5;
    }
}
