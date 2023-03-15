using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationController : MonoBehaviour
{
    public event Action<GameObject> OnAnimationEnded;
    public Animator Animator;
    
    public void AnimationEnded()
    {
        OnAnimationEnded?.Invoke(gameObject);
    }
}
