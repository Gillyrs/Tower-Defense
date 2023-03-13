using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private AnimationController controller;

    private void Awake()
    {
        controller.OnAnimationEnded += (gameObject) => gameObject.SetActive(false);
        controller.Animator.Play("LoadingEnd");
    }
}
