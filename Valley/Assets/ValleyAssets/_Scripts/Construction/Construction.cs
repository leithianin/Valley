using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Construction : MonoBehaviour
{
    // The cost of the contruction.
    [SerializeField] private int cost;

    [SerializeField, Tooltip("Actions to play when the construction is placed.")] private UnityEvent PlayOnPlace;
    [SerializeField, Tooltip("Actions to play when the construction is deleted.")] private UnityEvent PlayOnDelete;
    [SerializeField, Tooltip("Actions to play when the construction is selected.")] private UnityEvent PlayOnSelect;

    /// <summary>
    /// Return the Tools used for this construction.
    /// </summary>
    /// <returns>The Tool linked to the construction.</returns>
    public abstract SelectedTools LinkedTool();

    /// <summary>
    /// Used to do specific action when a construction is placed.
    /// </summary>
    /// <param name="position">The position where the construction is placed.</param>
    protected abstract void OnPlaceObject(Vector3 position);

    /// <summary>
    /// Used to do specific action when a construction is removed.
    /// </summary>
    protected abstract void OnRemoveObject();

    /// <summary>
    /// Used to do specific action when a construction is selected.
    /// </summary>
    protected abstract void OnSelectObject();

    /// <summary>
    /// Get the cost of the construction.
    /// </summary>
    public int GetCost => cost;

    /// <summary>
    /// Play the feedbacks and special actions when the object is placed.
    /// </summary>
    /// <param name="position">The position where the object is placed.</param>
    public void PlaceObject(Vector3 position)
    {
        PlayOnPlace?.Invoke();
        OnPlaceObject(position);
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void RemoveObject()
    {
        PlayOnDelete?.Invoke();
        OnRemoveObject();
    }

    /// <summary>
    /// Play the feedbacks and special actions when the object is removed.
    /// </summary>
    public void SelectObject()
    {
        PlayOnSelect?.Invoke();
        OnSelectObject();
    }

    //CODE REVIEW : Useless.
    public void SpawnObject(Vector3 position)
    {

    }

    /// <summary>
    /// Destroy the object.
    /// </summary>
    public void DespawnObject()
    {
        Destroy(gameObject);
    }
}
