using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using bambong; 

public class SelectButton : MonoBehaviour
{
    public Button button;

    [SerializeField] private Image img_SelectShow;

    public void SelectShow(bool active)
    {
        img_SelectShow.gameObject.SetActive(active);
    }
}
