using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera buildingCamera;
    public void MoveToBuildingCamera()
    {
        while(Camera.main.orthographicSize < buildingCamera.orthographicSize)
        {
            
        }
    }
}
