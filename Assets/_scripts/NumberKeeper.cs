using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

enum NumberKeeperState
{
    KeyBoardNumber,
    LevelNumber
}

public class NumberKeeper : MonoBehaviour
{
    [SerializeField] private NumberKeeperState _state;

    void Start()
    {
    }

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                (i + 1).ToString();
            transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
        if (_state == NumberKeeperState.KeyBoardNumber)
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<TextMeshProUGUI>().text = "0";
        if (_state == NumberKeeperState.LevelNumber)
        {
            UpdateLevelsButtons();
        }
    }

    public void UpdateLevelsButtons()
    {
        UserData uData = GameManager.I.GetUserData();

        for (int j = uData.LastUnlockedLevel + 1 /* chon baAd az akharin level ro kar darim*/;
             j < transform.childCount;
             j++)
        {
            transform.GetChild(j).GetComponent<Button>().interactable = false;
        }

        transform.GetChild(uData.LastUnlockedLevel).transform.GetChild(1).gameObject.SetActive(true);
    }
}