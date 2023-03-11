using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour, IInput
{
    public static BuildingSystem Current;
    [SerializeField] private bool isInBuildingMode;
    private Grid grid;
    [SerializeField] private GridLayout gridLayout;
    [SerializeField] private Tilemap mainTileMap;
    [SerializeField] private TileBase whiteTile;
    [SerializeField] private GameObject buildingGrid;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Building objectToPlace;
    [SerializeField] private AstarPath astarPath;
    private IInput pastInput;
    private void Awake()    
    {
        Current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    public void Activate(IInput pastInput)
    {
        CameraController.Current.MovetoBuildingCamera();
        ToggleBuildingMode();
        this.pastInput = pastInput;
        GameInput.Input.ChangeInput(BuildingInput, this);
    }
    public void Deactivate()
    {
    }
    private void BuildingInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            objectToPlace = Instantiate(prefab).GetComponent<Building>();
            BeginPlacing(objectToPlace);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Place();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleBuildingMode();
            pastInput.Activate(this);
        }
    }
    public void ToggleBuildingMode()
    {      
        isInBuildingMode = !isInBuildingMode;
        buildingGrid.SetActive(isInBuildingMode);
    }
    public void BeginPlacing<T>(T placeableObject) where T : IObject, IPlaceable
    {
        var transform = placeableObject.GetTransform();
        StartCoroutine(StartChangingPosition(transform));
    }
    private IEnumerator StartChangingPosition(Transform transform)
    {
        while (true)
        {
            transform.position = GetSnappedToGridPosition(GetMousePosition2D());
            yield return null;
        }
    }
    public static Vector3 GetMousePosition2D()
    {
        Ray ray3d = Camera.main.ScreenPointToRay(Input.mousePosition);
        return new Vector2(ray3d.origin.x, ray3d.origin.y);
    }
    public Vector3 GetSnappedToGridPosition(Vector3 position)
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPosition);
        return position;
    }

    private void Place()
    {
        if (objectToPlace.TryPlace())
        {
            StopAllCoroutines();
            objectToPlace = null;
            astarPath.Scan();
        }
        
    }

}
