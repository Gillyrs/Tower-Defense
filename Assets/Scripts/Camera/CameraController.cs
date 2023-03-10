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
    private void Awake()
    {
        Current = this;        
    }
    public void MovetoBuildingCamera()
    {
        animator.Play("GotoBuildingCamera");
    }
    public void MovetoPlayerCamera()
    {
        if(Camera.main.transform.position.x == player.transform.position.x 
           || Camera.main.transform.position.y == player.transform.position.y)
        {
            animator.Play("GotoPlayerCamera");
        }
        else
        {
            StartCoroutine(MoveCameraToPlayer(() => animator.Play("GotoPlayerCamera")));
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
