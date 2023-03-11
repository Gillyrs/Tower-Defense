using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cysharp.Threading.Tasks;
public class Unit : MonoBehaviour, IDamagable
{
    [SerializeField] private AILerp aiLerp;
    [SerializeField] private float radius;
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    private Transform playerPosition;
    private Rigidbody2D rb;
    private PlayerCore player;
    private int health = 100;
    private bool isAttacking;
    private void Start()
    {
        player = FindObjectOfType<PlayerCore>();
        playerPosition = player.transform;
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, playerPosition.position) <= radius && !isAttacking)
        {
            aiLerp.isStopped = true;
            StartCoroutine(StartAttack());
        }
        aiLerp.destination = playerPosition.position;
    }
    private IEnumerator StartAttack()
    {
        isAttacking = true;
        while (Vector3.Distance(transform.position, playerPosition.position) <= radius)
        {
            Debug.Log("Attacked");
            player.TakeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
    }
    public async void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
        aiLerp.isStopped = true;
        await UniTask.Delay(100);
        aiLerp.isStopped = false;
    }

}
