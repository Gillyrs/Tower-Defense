using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour, IInput
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 mousePosition;
    private bool isDeactivated;

    private void Start()
    {
        GameInput.Input.ChangeInput(PlayerInput, this);
        rb = GetComponent<Rigidbody2D>();
    }
    public void Activate(IInput pastInput)
    {
        isDeactivated = false;
        CameraController.Current.MovetoPlayerCamera();
        GameInput.Input.ChangeInput(PlayerInput, this);
    }
    public void Deactivate()
    {
        isDeactivated = true;
    }
    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildingSystem.Current.Activate(this);
        }
        else if (Input.GetMouseButton(0))
        {

        }
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        if (isDeactivated)
            return;

        rb.MovePosition(transform.position
                               + new Vector3(horizontalMovement, verticalMovement) 
                               * speed 
                               * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

    }
}
