using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerCore : MonoBehaviour, IInput, IObject, IDamagable
{
    [SerializeField] private float speed;
    [SerializeField] private Weapon weapon;
    [SerializeField] private float offset;
    [SerializeField] private int health = 100;
    [SerializeField] private Text healthText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private AnimationController menuLoading;
    [SerializeField] private AnimationController restartLoading;
    private Rigidbody2D rb;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 mousePosition;
    private bool isDeactivated;
    private void Start()
    {
        menuLoading.OnAnimationEnded += (gameObject) => GoMenu();
        restartLoading.OnAnimationEnded += (gameObject) => PlayAgain();
        GameInput.Input.ChangeInput(PlayerInput, this);
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        healthText.text = $"{health}";
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameInput.Input.ChangeInput(null, null);
            losePanel.SetActive(true);
        }
        
    }
    public void Activate(IInput pastInput)
    {
        isDeactivated = false;
        CameraController.Current.MovetoPlayerCamera();
        GameInput.Input.ChangeInput(PlayerInput, this);
    }
    public void Deactivate()
    {
        weapon.EndShooting();
        isDeactivated = true;
    }
    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildingSystem.Current.Activate(this);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            weapon.StartShooting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.EndShooting();
        }
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        if (isDeactivated)
            return;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.MovePosition(transform.position
                               + new Vector3(horizontalMovement, verticalMovement) 
                               * speed 
                               * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle + offset;

    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
