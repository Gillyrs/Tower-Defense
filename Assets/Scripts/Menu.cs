using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private AnimationController controller;
    private void Awake()
    {
        controller.OnAnimationEnded += StartLoading;
    }
    public void StartLoading(GameObject obj)
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
