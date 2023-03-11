using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Building : MonoBehaviour, IObject, IPlaceable
{
    [SerializeField] private bool isPlaced;
    [SerializeField] private bool canPlace;
    [SerializeField] private BoxCollider2D rangeCollider;
    public void Start()
    {
        var colliders = Physics2D.OverlapBoxAll(rangeCollider.transform.position, rangeCollider.size, 0).ToList();
        foreach (var item in colliders)
        {

        }
        if(colliders.Where(obj => obj.gameObject.layer == 
                           LayerMask.NameToLayer("Building"))
                           .ToList().Count > 0)
        {
            Debug.LogWarning("YEEEEEEES");
            canPlace = false;
        }
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public bool TryPlace()
    {
        if (!canPlace)
            return false;
        gameObject.layer = LayerMask.NameToLayer("Building");
        isPlaced = true;
        return true;
    }
    private void ToggleBuildingState(BuildingState buildingState)
    {
        if (buildingState == BuildingState.CantBuild)
            canPlace = false;
        if (buildingState == BuildingState.CanBuild)
            canPlace = true;
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
