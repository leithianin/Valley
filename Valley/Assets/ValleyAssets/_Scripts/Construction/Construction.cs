using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] private float cost;

    [SerializeField] private UnityEvent PlayOnPlace;

    protected abstract bool OnPlaceObject(Vector3 position);

    protected abstract void OnRemoveObject();

    public bool PlaceObject(Vector3 position)
    {
        PlayOnPlace?.Invoke();
        return OnPlaceObject(position);
    }

    public void RemoveObject()
    {
        OnRemoveObject();
    }

    public void SpawnObject(Vector3 position)
    {

    }

    public void DespawnObject()
    {
        
    }
}
