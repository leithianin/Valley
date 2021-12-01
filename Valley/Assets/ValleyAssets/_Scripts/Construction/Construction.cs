using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Construction : MonoBehaviour
{
    [SerializeField] private float cost;

    [SerializeField] private UnityEvent PlayOnPlace;
    [SerializeField] private UnityEvent PlayOnDelete;
    [SerializeField] private UnityEvent PlayOnSelect;

    public abstract SelectedTools LinkedTool();

    // Vérifier si on peut placer l'objet

    protected abstract bool OnCanPlaceObject(Vector3 position);

    protected abstract void OnPlaceObject(Vector3 position);

    protected abstract void OnRemoveObject();

    protected abstract void OnSelectObject();

    public bool CanPlaceObject(Vector3 position)
    {
        bool toReturn = true;

        NavMeshHit hit;
        if(!NavMesh.SamplePosition(position, out hit, .5f, NavMesh.AllAreas))
        {
            toReturn = false;
        }

        return toReturn && OnCanPlaceObject(position);
    }

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
        
    }
}
