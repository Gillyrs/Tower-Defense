using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IObject, IPlaceable
{
    private bool isPlaced;
    public Transform GetTransform()
    {
        return transform;
    }
    public void Place()
    {
        
        Debug.Log("Placed");
    }
    private void ToggleBuildingState(BuildingState buildingState)
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPlaced)
        {
            ToggleBuildingState(BuildingState.CantBuild);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPlaced)
        {
            ToggleBuildingState(BuildingState.CanBuild);
        }
    }


}

public enum BuildingState
{
    CanBuild,
    CantBuild
}
