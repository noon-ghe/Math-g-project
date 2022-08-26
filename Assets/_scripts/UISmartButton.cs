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
    Setting,
    DisplayLevels,

    Share,
    Home,
    Exit,
    None

    //...
}

public class UISmartButton : MonoBehaviour
{
    [SerializeField] private ButtonState state;
    private UserData _userData;

    void Start()
    {
        if (GetComponent<Button>())
        {
            GetComponent<Button>().onClick.AddListener(MyButtonClicked);
        }
        else if (GetComponent<Toggle>())
        {
            GetComponent<Toggle>().onValueChanged.AddListener(MyToggleClicked);
        }
    }

    private void OnEnable()
    {
        //Transition.OnTransition += SetButtonState;
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

    void MyToggleClicked(bool isOn)
    {
        UserData uData = GameManager.I.GetUserData();
        uData.SoundOnOrOff = isOn;
        GameManager.I.SaveGame(uData);
    }

    async void MyButtonClicked()
    {
        AudioClip audioClipToPlay;

        switch (state)
        {
            case ButtonState.Enter:
                audioClipToPlay = Riddle.I.CheckAnswer()
                    ? SoundManager.I.CorrectAnswerSound
                    : SoundManager.I.WrongAnswerSound;
                SoundManager.I.PlayClickSound(audioClipToPlay);
                break;
            default:
                audioClipToPlay = SoundManager.I.NormalClickSound;
                SoundManager.I.PlayClickSound(audioClipToPlay);
                switch (state)
                {
                    case ButtonState.None:
                        print("none option");
                        break;
                    case ButtonState.Back:
                        GameManager.I.Back();
                        break;
                    case ButtonState.LevelSelector:
                        Riddle.I.SelectLevel(int.Parse(GetComponentInChildren<TextMeshProUGUI>().text) - 1);
                        break;
                    case ButtonState.KeyboardNumber:
                        Riddle.I.UpdateUserAnswer(GetComponentInChildren<TextMeshProUGUI>().text);
                        break;
                    case ButtonState.Clear:
                        Riddle.I.ClearInputField();
                        break;
                    case ButtonState.StartGame:
                        GameManager.I.UpdateGameState(GameStates.Game);
                        break;
                    case ButtonState.DisplayLevels:
                        GameManager.I.UpdateGameState(GameStates.Levels);
                        break;
                    case ButtonState.Setting:
                        GameManager.I.UpdateGameState(GameStates.Setting);
                        break;
                    case ButtonState.Share:
                        Debug.Log("sharing");
                        break;
                    case ButtonState.Home:
                        GameManager.I.UpdateGameState(GameStates.Home);
                        break;
                    case ButtonState.Exit:
                        //Application.Quit();
                        print("Quit");
                        break;
                    case ButtonState.SelectLevel:
                        GameManager.I.UpdateGameState(GameStates.Game);
                        await Task.Delay(
                            TimeSpan.FromSeconds(Transition.I
                                .GetHalfOfTransitionTime())); //for sure that gamestate updated and Riddle are active
                        Riddle.I.SelectLevel(int.Parse(GetComponentInChildren<TextMeshProUGUI>().text) - 1);
                        break;
                }

                break;
        }
    }

    /*public void SetButtonState(bool bstate)
    {
        if (state != ButtonState.LevelSelector)
        {
            //GetComponent<Button>().interactable = bstate;
        }
    }*/
}