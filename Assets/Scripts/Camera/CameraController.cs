using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController Current;
    [SerializeField] private PlayerCore player;
    [SerializeField] Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [SerializeField] private AudioSource slideSound;
    private void Awake()
    {
        Current = this;        
    }
    private void OnValidate()
    {
        if(isActiveAndEnabled)
            animator.speed = speed;
    }
    public void MovetoBuildingCamera()
    {
        slideSound.Play();
        animator.Play("GotoBuildingCamera");
    }
    public void MovetoPlayerCamera()
    {
        if (Camera.main.transform.position.x == player.transform.position.x
           || Camera.main.transform.position.y == player.transform.position.y)
        {
            slideSound.Play();            
            animator.Play("GotoPlayerCamera");
        }
        else
        {
            StartCoroutine(MoveCameraToPlayer(() => { slideSound.Play(); animator.Play("GotoPlayerCamera"); } ));
        }
        
    }

    private IEnumerator MoveCameraToPlayer(Action action)
    {
        Vector3 startPosition = Camera.main.transform.position;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            Camera.main.transform.position = Vector2.Lerp(startPosition, player.transform.position, timeElapsed / duration);
            Camera.main.transform.position += new Vector3(0, 0, -10);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        action.Invoke();
    }
}
