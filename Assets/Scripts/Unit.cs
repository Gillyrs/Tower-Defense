using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    public void TakeDamage(int damage)
    {
        Debug.Log("Took damage");
    }
}
