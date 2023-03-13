using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour
{
    public static Economy Current;
    [field:SerializeField] public int Money { get; private set; }
    [SerializeField] private Text moneyText;
    private void Awake()
    {
        Current = this;
    }

    private void Update()
    {
        moneyText.text = $"{Money}";
    }
    public void IncreaseMoney(int amount)
    {
        Money += amount;
    }
    public void DescreaseMoney(int amount)
    {
        Money -= amount;
        if (Money < 0)
            Money = 0;
    }
}
