using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;

    private void Update()
    {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, -10);
    }
}
