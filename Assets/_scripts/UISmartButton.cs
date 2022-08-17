using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum ButtonState
{
    KeyboardNumber,
    Enter,
    Clear,
    LevelSelector,

    Back,
    StartGame,
    SelectLevel,

    DisplayLevels
    //...
}

public class UISmartButton : MonoBehaviour
{
    [SerializeField] private ButtonState state;
    private UserData _userData;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(MyButtonClicked);
    }

    private void OnEnable()
    {
        Transition.OnTransition += SetButtonState;
        if (state == ButtonState.KeyboardNumber)
        {
            GetComponent<Button>().interactable = true;
        }
        //Riddle.OnRiddleStateChanged += SetButtonState;
    }

    private void OnDisable()
    {
        //Riddle.OnRiddleStateChanged -= SetButtonState;
    }


    async void MyButtonClicked()
    {
        switch (state)
        {
            case ButtonState.KeyboardNumber:
                Riddle.I.UpdateUserAnswer(GetComponentInChildren<TextMeshProUGUI>().text);
                break;
            case ButtonState.Enter:
                Riddle.I.CheckAnswer();
                break;
            case ButtonState.Clear:
                Riddle.I.ClearInputField();
                break;
            case ButtonState.LevelSelector:
                Riddle.I.SelectLevel(int.Parse(GetComponentInChildren<TextMeshProUGUI>().text) - 1);
                break;
            case ButtonState.Back:
                GameManager.I.Back();
                break;
            case ButtonState.StartGame:
                GameManager.I.UpdateGameState(GameStates.Game);
                break;
            case ButtonState.DisplayLevels:
                GameManager.I.UpdateGameState(GameStates.Levels);
                break;
            case ButtonState.SelectLevel:
                GameManager.I.UpdateGameState(GameStates.Game);
                await Task.Delay(
                    TimeSpan.FromSeconds(Transition.I
                        .GetHalfOfTransitionTime())); //for sure that gamestate updated and Riddle are active
                Riddle.I.SelectLevel(int.Parse(GetComponentInChildren<TextMeshProUGUI>().text) - 1);
                break;
        }
    }

    public void SetButtonState(bool bstate)
    {
        if (state != ButtonState.LevelSelector)
        {
            //GetComponent<Button>().interactable = bstate;
        }

    }
}