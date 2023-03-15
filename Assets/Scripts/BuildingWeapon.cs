using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWeapon : Weapon
{
    [SerializeField] private float weaponrotationSpeed;
    [SerializeField] private bool isBusy;
    [SerializeField] private Unit target;
    [SerializeField] private Vector3 lastPosition;
    [SerializeField] private List<Unit> unitQueue;
    [SerializeField] private Building building;
    private Coroutine coroutine;
    private Coroutine shootingCoroutine;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var sounds = FindObjectsOfType<AudioSource>();
        foreach (var item in sounds)
        {
            if (item.name == "Hitting Sound")
                hittingSound = item;
        }
    }
    private void FixedUpdate()
    {
        if (!building.isPlaced)
            return;
        if (target != null)
        {
            if (target.transform.position != lastPosition)
            {
                if (coroutine != null)
                    StopCoroutine(coroutine);
                lastPosition = target.transform.position;
                
                coroutine = StartCoroutine(Rotate(target.transform, weaponrotationSpeed));
            }
        }
    }
    public IEnumerator Rotate(Transform target, float weaponrotationSpeed)
    {
        isBusy = true;
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        while (Mathf.Abs(rb.rotation - angle) > 2f)
        {
            float step = Mathf.MoveTowardsAngle(rb.rotation, angle, weaponrotationSpeed * Time.deltaTime);
            rb.rotation = step;
            yield return null;
        }
        StartShooting();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!building.isPlaced)
            return;
        if (collision.TryGetComponent(out Unit enemy))
        {
            if (!isBusy)
            {
                enemy.OnUnitDestroyed += OnTargetDestroyed;
                target = enemy;
                lastPosition = enemy.transform.position;
                coroutine = StartCoroutine(Rotate(enemy.transform, weaponrotationSpeed));
            }
            else if(!unitQueue.Contains(enemy))
            {
                enemy.OnUnitDestroyed += OnTargetInQueueDestroyed;
                unitQueue.Add(enemy);
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Unit enemy))
        {
            if (enemy == target)
                OnTargetDestroyed(target);
            else if (unitQueue.Contains(enemy))
                OnTargetInQueueDestroyed(enemy);
        }
    }

    private void OnTargetDestroyed(Unit target)
    {
        EndShooting();  
        target.OnUnitDestroyed -= OnTargetDestroyed;
        this.target = null;
        isBusy = false;
        if (unitQueue.Count > 0 && this.target == null)
        {
            isBusy = true;
            var unit = unitQueue[0];
            unitQueue.RemoveAt(0);
            unit.OnUnitDestroyed -= OnTargetInQueueDestroyed;
            unit.OnUnitDestroyed += OnTargetDestroyed;
            this.target = unit;
            lastPosition = unit.transform.position;
            coroutine = StartCoroutine(Rotate(unit.transform, weaponrotationSpeed));
        }
    }
    private void OnTargetInQueueDestroyed(Unit target)
    {
        target.OnUnitDestroyed -= OnTargetInQueueDestroyed;
        unitQueue.Remove(target);
    }
}
