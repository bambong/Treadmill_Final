using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectDataButton : MonoBehaviour
{
    [SerializeField] private GameObject gameData_2;
    [SerializeField] private GameObject gameData_3;

    public void GameDataButton(int open)
    {
        if (open == 2)
            gameData_2.SetActive(true);
        else if (open == 3)
            gameData_3.SetActive(true);
        else if (open == 4)
            gameData_2.SetActive(false);
        else if (open == 5)
            gameData_3.SetActive(false);
    }
}
