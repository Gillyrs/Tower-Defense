using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 mousePosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(transform.position
                               + new Vector3(horizontalMovement, verticalMovement) 
                               * speed 
                               * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePosition - rigidbody.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rigidbody.rotation = angle;

    }
}
