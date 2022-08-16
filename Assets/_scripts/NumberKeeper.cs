using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                (i + 1).ToString();
        }

        if (_state == NumberKeeperState.KeyBoardNumber)
            transform.GetChild(transform.childCount - 1).GetComponentInChildren<TextMeshProUGUI>().text = "0";
    }
}