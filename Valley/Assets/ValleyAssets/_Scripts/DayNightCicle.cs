using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCicle : MonoBehaviour
{
    [SerializeField] private float dayLifespan = 50;

    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 rotationValueForOneDay;

    [SerializeField] private Transform directionalLightTransform;
    [SerializeField] private HDAdditionalLightData directionalLight;
    [SerializeField] private Gradient gradient;
    [SerializeField] private AnimationCurve temperatureVariation;
    [SerializeField] private int minimalTemperature;
    [SerializeField] private int maximalTemperature;


    [SerializeField] private float currentDayHour;

    private float GetCurrentDayPercent => currentDayHour / dayLifespan;

    private void Update()
    {
        currentDayHour += Time.deltaTime;
        if(currentDayHour > dayLifespan)
        {
            currentDayHour -= dayLifespan;
        }

        directionalLightTransform.eulerAngles = new Vector3((startRotation.x + rotationValueForOneDay.x * GetCurrentDayPercent) % 180, startRotation.y + rotationValueForOneDay.y * GetCurrentDayPercent, startRotation.z + rotationValueForOneDay.z * GetCurrentDayPercent);

        directionalLight.SetColor(gradient.Evaluate(GetCurrentDayPercent), temperatureVariation.Evaluate(GetCurrentDayPercent) * (maximalTemperature - minimalTemperature) + minimalTemperature);
    }
}
