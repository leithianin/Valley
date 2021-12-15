using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public abstract class ConstructionPreview : MonoBehaviour
{
    // The Construction linked to the preview.
    [SerializeField] protected Construction realConstruction;

    // A list of all the object blocking the placement of the construction.
    protected List<GameObject> objectBlockingPose = new List<GameObject>();

    // The senvitivity with which the preview will check the Navmesh.
    [SerializeField] protected float navMeshSensitivity = .5f;

    // The mesh renderer of the preview.
    [SerializeField] private MeshRenderer mesh;

    // The preview material when the player can place the object.
    [SerializeField] protected Material availableMaterial;
    // The preview material when the player can't place the object.
    [SerializeField] protected Material unavailableMaterial;

    [SerializeField, Tooltip("Feedbacks to play when the player succeed to place the object.")] protected UnityEvent PlayOnAskToPlaceTrue;
    [SerializeField, Tooltip("Feedbacks to play when the player try to place the object without being able to.")] protected UnityEvent PlayOnAskToPlaceFalse;
    [SerializeField, Tooltip("Feedbacks to play when the player hasn't enough ressources to place the construction.")] protected UnityEvent PlayOnAskToPlaceNoMoney;

    protected bool availabilityState = true;

    /// <summary>
    /// Getter for the Construction.
    /// </summary>
    public Construction RealConstruction => realConstruction;

    /// <summary>
    /// Used to do specific action when we want to check if the construction can be placed.
    /// </summary>
    /// <param name="position">The position of the construction preview.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    protected abstract bool OnCanPlaceObject(Vector3 position);

    /// <summary>
    /// Used to do specific action when the player try to place the construction.
    /// </summary>
    /// <param name="position">The position where the player want to place the construction.</param>
    protected abstract void OnAskToPlace(Vector3 position);

    /// <summary>
    /// Called when the player try to place the construction.
    /// </summary>
    /// <param name="position">The position where the player want to place the construction.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    public bool AskToPlace(Vector3 position)
    {
        bool canPlace = CanPlaceObject(position);
        OnAskToPlace(position);

        if (RessourcesManager.GetCurrentWoods < realConstruction.GetCost)
        {
            PlayOnAskToPlaceNoMoney?.Invoke();
        }

        if (canPlace)
        {
            PlayOnAskToPlaceTrue?.Invoke();
        }
        else
        {
            PlayOnAskToPlaceFalse?.Invoke();
        }
        return canPlace;
    }

    /// <summary>
    /// Called when we want to check if the construction can be placed at the specified position.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>Return true if the player can place the construction.</returns>
    public bool CanPlaceObject(Vector3 position)
    {
        bool toReturn = true;

        if(RessourcesManager.GetCurrentWoods < realConstruction.GetCost)
        {
            toReturn = false;
        }

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(position, out hit, 1/navMeshSensitivity, NavMesh.AllAreas)) //Check si on est sur un terrain praticable
        {
            toReturn = false;
        }

        if(objectBlockingPose.Count > 0)
        {
            toReturn = false;
        }

        return toReturn && OnCanPlaceObject(position);
    }

    /// <summary>
    /// Spawn the construction preview.
    /// </summary>
    /// <param name="position">The position of the spawn.</param>
    public void SpawnObject(Vector3 position)
    {

    }

    /// <summary>
    /// Destroy the construction preview GameObject.
    /// </summary>
    public void DespawnObject()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Update the material of the mesh depending of the availability.
    /// </summary>
    public void CheckAvailability()
    {
        if(CanPlaceObject(transform.position) != availabilityState)
        {
            availabilityState = CanPlaceObject(transform.position);
            if(availabilityState)
            {
                mesh.material = availableMaterial;
            }
            else
            {
                mesh.material = unavailableMaterial;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NavMeshObstacle>() != null && !objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectBlockingPose.Contains(other.gameObject))
        {
            objectBlockingPose.Remove(other.gameObject);
        }
    }
}
