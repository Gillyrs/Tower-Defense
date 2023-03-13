using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Cysharp.Threading.Tasks;
using System;

public class Unit : MonoBehaviour, IDamagable
{
    public event Action<Unit> OnUnitDestroyed;
    public ObjectPool Pool;
    [Header("Stats")]
    [SerializeField] private float radius;
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private bool isAttacking;
    [SerializeField] private int reward;

    [Header("Links")]
    [SerializeField] private AILerp aiLerp;
    
    private Transform playerPosition;
    private Rigidbody2D rb;
    private PlayerCore player;
    private bool isActiveted;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aiLerp.speed = speed;
    }
    private void FixedUpdate()
    {
        if (!isActiveted)
            return;
        if(Vector3.Distance(transform.position, playerPosition.position) <= radius && !isAttacking)
        {
            aiLerp.isStopped = true;
            StartCoroutine(StartAttack());
        }
        aiLerp.isStopped = false;
        aiLerp.destination = playerPosition.position;
    }
    public void ClearSubs()
    {
        OnUnitDestroyed = null;
    }
    public void Activate(PlayerCore player)
    {
        this.player = player;
        playerPosition = player.transform;
        isActiveted = true;
    }
    private IEnumerator StartAttack()
    {
        isAttacking = true;
        while (Vector3.Distance(transform.position, playerPosition.position) <= radius)
        {
            player.TakeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
        }
        isAttacking = false;
    }
    public async void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnUnitDestroyed?.Invoke(this);
            Economy.Current.IncreaseMoney(reward);
            isActiveted = false;
        }
        aiLerp.isStopped = true;
        await UniTask.Delay(100);
        aiLerp.isStopped = false;
    }

}
