using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class Listener : MonoBehaviour, IPointerClickHandler
{
    public event Action OnPointerClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClicked?.Invoke();
    }
}
