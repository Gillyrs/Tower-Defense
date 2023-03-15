using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameInput : MonoBehaviour
{
    public static GameInput Input { get; private set; }
    private Action currentInputAction;
    private IInput currentInput;
    private void Awake()
    {
        Input = this;
    }
    private void Update()
    {
        currentInputAction.Invoke();
    }
    public void ChangeInput(Action inputAction, IInput input)
    {
        
        currentInputAction = inputAction;
        currentInput?.Deactivate();
        currentInput = input;
    }
}
