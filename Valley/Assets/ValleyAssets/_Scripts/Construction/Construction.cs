using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] private int cost;

    [SerializeField] private UnityEvent PlayOnPlace;
    [SerializeField] private UnityEvent PlayOnDelete;
    [SerializeField] private UnityEvent PlayOnSelect;

    public abstract SelectedTools LinkedTool();

    protected abstract void OnPlaceObject(Vector3 position);

    protected abstract void OnRemoveObject();

    protected abstract void OnSelectObject();

    public int GetCost => cost;

    public void PlaceObject(Vector3 position)
    {
        PlayOnPlace?.Invoke();
        OnPlaceObject(position);
    }

    public void RemoveObject()
    {
        PlayOnDelete?.Invoke();
        OnRemoveObject();
    }

    public void SelectObject()
    {
        PlayOnSelect?.Invoke();
        OnSelectObject();
    }

    public void SpawnObject(Vector3 position)
    {

    }

    public void DespawnObject()
    {
        Destroy(gameObject);
    }
}
