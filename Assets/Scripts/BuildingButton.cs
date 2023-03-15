using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class BuildingButton : MonoBehaviour
{
    private Text text;
    [SerializeField] private int price;
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private Animator animator;
    private void Start()
    {
        if(animator != null)
            animator.GetComponent<AnimationController>().OnAnimationEnded += (gameObject) => { animator.gameObject.SetActive(false); animator.gameObject.SetActive(true); };
        text = GameObject.FindGameObjectWithTag("Price").GetComponent<Text>();
    }
    private void OnMouseEnter()
    {
        text.text = $"{price}";
    }
    public void Startplacing()
    {
        if(Economy.Current.Money < price)
        {
            Debug.Log("Cant");
            animator.Play("NotEnoughMoney");
            return;
        }
            
        var objectToPlace = Instantiate(buildingPrefab).GetComponent<Building>();
        BuildingSystem.Current.BeginPlacingBuidling(objectToPlace);
    }
}

