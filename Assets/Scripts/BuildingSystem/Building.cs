using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Building : MonoBehaviour, IObject, IPlaceable
{
    public bool isPlaced;
    public int Price;
    [SerializeField] private bool canPlace;
    [SerializeField] private BoxCollider2D rangeCollider;
    [SerializeField] private Color canBuild;
    [SerializeField] private Color cantBuild;
    [SerializeField] private Color defaultBuild;
    [SerializeField] private List<SpriteRenderer> renderers;
    
    public void Start()
    {
        if (isPlaced)
            return;
        ToggleBuildingState(BuildingState.CanBuild);
        var colliders = Physics2D.OverlapBoxAll(rangeCollider.transform.position, rangeCollider.size, 0).ToList();
        if(colliders.Where(obj => obj.gameObject.layer == 
                           LayerMask.NameToLayer("Building"))
                           .ToList().Count > 0)
        {
            ToggleBuildingState(BuildingState.CantBuild);
        }
    }
    private void Update()
    {
        if (!isPlaced)
            Debug.Log(canPlace);
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
        foreach (var item in renderers)
        {
            item.color = defaultBuild;
        }
        return true;
    }
    private void ToggleBuildingState(BuildingState buildingState)
    {
        if (buildingState == BuildingState.CantBuild)
        {
            foreach (var item in renderers)
            {
                item.color = cantBuild;
            }
            canPlace = false;
        }
        if (buildingState == BuildingState.CanBuild)
        {
            foreach (var item in renderers)
            {
                item.color = canBuild;
            }
            canPlace = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlaced && collision.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            Debug.Log("Cant build");
            ToggleBuildingState(BuildingState.CantBuild);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isPlaced)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Building"))
                ToggleBuildingState(BuildingState.CantBuild);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited from " + collision.name);
        if (!isPlaced && collision.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            Debug.Log("Can build");
            ToggleBuildingState(BuildingState.CanBuild);
        }
    }


}

public enum BuildingState
{
    CanBuild,
    CantBuild
}
