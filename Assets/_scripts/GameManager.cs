using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public enum GameStates
{
    Home,
    Setting,
    Levels,
    Game,
    Win,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject HomeCanvas,SettingCanvas,GameCanvas, LevelsCanvas, WinCanvas;
    static GameManager _i; //  _i ←→ _instance 
    private List<GameStates> _history = new List<GameStates>();
    private static string path;
    private UserData _userData;


    public static GameManager I
    {
        //  _i ←→ Instance 
        get
        {
            //Singleton
            if (_i == null) //Are we have Riddle before? Checking...
            {
                _i = FindObjectOfType<GameManager>();
                if (_i == null)
                {
                    //Ok... we dont have any <Riddle>(); then create that
                    GameObject gManager = new GameObject("Game Manager");
                    gManager.AddComponent<GameManager>();
                    _i = gManager.GetComponent<GameManager>();
                }
            }

            //return _i anyway
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null) // if we have _i (GameManager) before, then destroy me bos...
            Destroy(this);
        DontDestroyOnLoad(this.gameObject); //it's ok... i'am first <GameManager>() now.
        //LoadGame(_userData, path);
        path = Application.persistentDataPath + "/UserData.SavedData";
        _userData = LoadGame(_userData);
    }

    private void Start()
    {
        //UpdateGameState(GameStates.Home);
        EnableCanvases(GameStates.Home);
        _history.Add(GameStates.Home);
    }

    public void UpdateGameState(GameStates newState)
    {
        DoStateHandlesSwitches(newState);
        StartCoroutine(UpdateCanvases(newState));
    }

    void DoStateHandlesSwitches(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.Home:
                HandelHome(newState);
                break;
            case GameStates.Levels:
                HandelLevels(newState);
                break;
            case GameStates.Setting:
                HandelSetting(newState);
                break;
            case GameStates.Game:
                HandleGame(newState);
                break;
            case GameStates.Win:
                HandleWin(newState);
                break;
        }
    }

    private void HandelHome(GameStates newState)
    {
        print("History deleted!");
        _history.Clear();
    }

    private void HandleWin(GameStates newState)
    {
        _history.Remove(_history[_history.Count - 1]); //delete Game
        _history.Remove(_history[_history.Count - 1]); //delete Levels
    }

    private void HandleGame(GameStates newState)
    {
        if (_history[_history.Count - 1] != GameStates.Levels)
            _history.Add(GameStates.Levels);
        _history.Add(newState);
        print(GameStates.Levels + " added to history");
        print(newState + " added to history");
    }

    private void HandelLevels(GameStates newState)
    {
        _history.Add(newState);
        _userData = LoadGame(_userData);
        print(newState + " added to history");
        //TODO Scroll near last unlocked level
    }
    private void HandelSetting(GameStates newState)
    {
        _history.Add(newState);
        _userData = LoadGame(_userData);
        print(newState + " added to history");
    }

    public void Back()
    {
        print(_history[_history.Count - 1] + " removed");
        _history.Remove(_history[_history.Count - 1]);
        StartCoroutine(UpdateCanvases(_history[_history.Count - 1]));
    }

    IEnumerator UpdateCanvases(GameStates newState)
    {
        Transition.I.FadeOut();
        yield return new WaitForSeconds(Transition.I.GetHalfOfTransitionTime());
        EnableCanvases(newState);
        //TODO WinCanvas.SetActive(true);
        Transition.I.FadeIn();
    }

    private void EnableCanvases(GameStates newState)
    {
        if (newState == GameStates.Levels)
            HandelLevels(newState);

        HomeCanvas.SetActive(newState == GameStates.Home);
        SettingCanvas.SetActive(newState == GameStates.Setting);
        LevelsCanvas.SetActive(newState == GameStates.Levels);
        GameCanvas.SetActive(newState == GameStates.Game);
        WinCanvas.SetActive(newState == GameStates.Win);
    }

    public UserData LoadGame(UserData uData)
    {
        SaveLoadSystem.Load(
            path,
            (UserData data) => { uData = data; },
            () => { print("Load Success Game manager"); },
            () =>
            {
                uData = new UserData();
                //SaveGame(uData);
            },
            (Exception e) => { print(e); });

        return uData;
    }

    public void SaveGame(UserData data)
    {
        SaveLoadSystem.Save(data, path, () => { print("Success Save"); }, (Exception e) => { print(e); });
        //...
    }

    public UserData GetUserData()
    {
        return _userData;
    }
}

[System.Serializable]
public class UserData
{
    public bool _isEmpty = true;
    public int LastUnlockedLevel = 0;
    public int SelectedTheme;
    public float Volume;
    public bool SoundOnOrOff;

    public UserData(bool isEmpty, int lastUnlockedLevel, int selectedTheme, float volume, bool soundOn)
    {
        _isEmpty = isEmpty;
        LastUnlockedLevel = lastUnlockedLevel;
        SelectedTheme = selectedTheme;
        Volume = volume;
        SoundOnOrOff = soundOn;
    }

    public UserData()
    {
        _isEmpty = false;
        LastUnlockedLevel = 0;
        SelectedTheme = 0;
        Volume = 100;
        SoundOnOrOff = true;
    }
}