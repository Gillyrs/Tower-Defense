using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator.Play("LoadingEnd");
    }
}
