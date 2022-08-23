using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Riddle : MonoBehaviour
{
    RiddleSO[] riddle;

    [SerializeField] private Image riddleImage;
    [SerializeField] private TextMeshProUGUI reactToAnswerTex;
    [SerializeField] private TMP_InputField inputField;
    int _selectedLevelIndex = 0;
    private UserData _userData;
    private string path;
    bool isAnswerCorrect = false;

    static Riddle _i; //  _i ←→ _instance 
    //public static event Action<bool> OnRiddleStateChanged;

    public static Riddle I
    {
        //  I ←→ Instance 
        get
        {
            //Singleton
            if (_i == null) //Are we have Riddle before? Checking...
            {
                _i = FindObjectOfType<Riddle>();
                if (_i == null)
                {
                    //Ok... we dont have any <Riddle>(); then create that
                    GameObject rdl = new GameObject("Game Manager");
                    rdl.AddComponent<Riddle>();
                    _i = rdl.GetComponent<Riddle>();
                }
            }

            //return _i anyway
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null) // if we have _i (Riddle) before, then destroy me bos...
            Destroy(this);
        DontDestroyOnLoad(this.gameObject); //it's ok... i'am first <Riddle>()
        LoadAllRiddleAssets();
    }

    private void Start()
    {
        _userData = GameManager.I.LoadGame(_userData);
    }

    private void OnEnable()
    {
        //TODO isLevelIndexSelected = GameManager.I.IsLevelIndexSelected;
        _userData = GameManager.I.LoadGame(_userData);
        inputField.text = "";
        SelectLastUnlockedLevel();
    }

    void SelectLastUnlockedLevel()
    {
        _selectedLevelIndex = _userData.LastUnlockedLevel;
        SelectLevel(_selectedLevelIndex);
    }

    public void UpdateUserAnswer(string text)
    {
        if (inputField.text.Length <= inputField.characterLimit)
            inputField.text += text;
    }

    public void ClearInputField()
    {
        inputField.text = string.Empty;
    }


    public void SelectLevel(int index)
    {
        if (_selectedLevelIndex + 1 < riddle.Length)
        {
            _selectedLevelIndex = index;
            DisplayRiddle();
        }
        else
        {
            print("Riddles Ended");
        }
    }

    void DisplayRiddle()
    {
        riddleImage.sprite = riddle[_selectedLevelIndex].GetRiddleSprite();
        //OnRiddleStateChanged?.Invoke(true);
    }

    public bool CheckAnswer()
    {
        StartCoroutine(CheckingAnswer());
        return isAnswerCorrect;
    }

    public IEnumerator CheckingAnswer()
    {
        if (inputField.text != "")
            if (inputField.text == riddle[_selectedLevelIndex].GetAnswere())
            {
                if (_userData.LastUnlockedLevel == _selectedLevelIndex)
                {
                    if (_userData.LastUnlockedLevel + 1 < riddle.Length)
                    {
                        _userData.LastUnlockedLevel += 1;
                    }

                    _userData._isEmpty = false;
                    GameManager.I.SaveGame(_userData);
                }

                isAnswerCorrect = true;
                ClearInputField();
                StartCoroutine(NextLevel());
            }
            else
            {
                reactToAnswerTex.text = "Think more ...!";
                isAnswerCorrect = false;
                ClearInputField();
                yield return new WaitForSeconds(0.7f);
                reactToAnswerTex.text = "";
            }
    }

    private IEnumerator NextLevel()
    {
        if (_selectedLevelIndex + 1 < riddle.Length)
        {
            Transition.I.FadeOut();
            yield return new WaitForSeconds(Transition.I.GetHalfOfTransitionTime());
            //OnRiddleStateChanged?.Invoke(false);
            SelectLevel((_selectedLevelIndex + 1));
            Transition.I.FadeIn();
        }
        else
        {
            print("Riddles Ended");
            GameManager.I.UpdateGameState(GameStates.Win);
        }
    }

    void LoadAllRiddleAssets()
    {
        object[] assets = Resources.LoadAll<RiddleSO>("Riddles");
        riddle = new RiddleSO[assets.Length];
        for (int i = 0; i < assets.Length; i++)
        {
            riddle[i] = (RiddleSO) assets[i];
        }
    }
}