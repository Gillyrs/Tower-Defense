using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private GameObject buildingMenu;
    [SerializeField] private Listener listener;
    private IInput pastInput;
    private void Awake()    
    {
        Current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    public void Activate(IInput pastInput)
    {
        listener.OnPointerClicked += TryPlace;
        CameraController.Current.MovetoBuildingCamera();
        ToggleBuildingMode();
        this.pastInput = pastInput;
        buildingMenu.SetActive(true);
        GameInput.Input.ChangeInput(BuildingInput, this);
    }
    public void Deactivate()
    {
        listener.OnPointerClicked -= TryPlace;
        buildingMenu.SetActive(false);
        if (objectToPlace == null)
            return;        
        Destroy(objectToPlace.gameObject);
    }
    private void BuildingInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleBuildingMode();
            pastInput.Activate(this);
        }
    }
    public void TryPlace()
    {
        if(objectToPlace != null)
        {
            Place();
        }
    }
    public void ToggleBuildingMode()
    {      
        isInBuildingMode = !isInBuildingMode;
        buildingGrid.SetActive(isInBuildingMode);
    }
    private void BeginPlacing<T>(T placeableObject) where T : IObject, IPlaceable
    {
        var transform = placeableObject.GetTransform();
        StartCoroutine(StartChangingPosition(transform));
    }
    public void BeginPlacingBuidling<T>(T placeableObject) where T : Building
    {
        if (objectToPlace != null)
            Destroy(objectToPlace.gameObject);
        else if (placeableObject.Price > Economy.Current.Money)
            return;
        Economy.Current.DescreaseMoney(placeableObject.Price);
        objectToPlace = placeableObject;
        var transform = placeableObject.GetTransform();
        StartCoroutine(StartChangingPosition(transform));
    }
    private IEnumerator StartChangingPosition(Transform transform)
    {
        while (transform != null)
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
            StartCoroutine(Scan());
        }
        
    }
    private IEnumerator Scan()
    {
        foreach (var item in AstarPath.active.ScanAsync())
        {
            yield return null;
        }
    }

}
