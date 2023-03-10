using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Camera Current;
    [SerializeField] private Camera buildingCamera;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Current = buildingCamera;
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Current = Camera.main;
        }
    }
    public void MoveToBuildingCamera()
    {
        Current = buildingCamera;
        while(Camera.main.orthographicSize < buildingCamera.orthographicSize)
        {
            
        }
    }
    /*public void MoveToBuildingCamera()
    {
        Current = Camera.main;
        while (Camera.main.orthographicSize < buildingCamera.orthographicSize)
        {

        }
    }*/
}
