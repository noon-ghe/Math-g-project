using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transition : MonoBehaviour
{
    private bool isFadeTime = false;
    static Transition _i; //  _i ←→ _instance 
    private CanvasGroup canvasGroup;
    public static Action<bool> OnTransition;
    private float _timer = 0;
    private bool fadeDir;

    public static Transition I
    {
        //  _i ←→ Instance 
        get
        {
            //Singleton
            if (_i == null) //Are we have Riddle before? Checking...
            {
                _i = FindObjectOfType<Transition>();
                if (_i == null)
                {
                    //Ok... we dont have any <Riddle>(); then create that
                    GameObject transition = new GameObject("TransitionManager");
                    transition.AddComponent<Transition>();
                    _i = transition.GetComponent<Transition>();
                }
            }

            //return _i anyway
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null) // if we have _i (Transition) before, then destroy me bos...
            Destroy(this);
        DontDestroyOnLoad(this.gameObject); //it's ok... i'am first <Transition>() now.
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }


    public void FadeIn() // شروع با صفحه سیاه و اتمام با صفحه بی رنگ (به سمت بی رنگی)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        _timer = GetHalfOfTransitionTime();
        fadeDir = false;
        isFadeTime = true;
    }

    public void FadeOut() // شروع با صفحه بی رنگ به اتمام با صفحه سیاه (به سمت رنگ سیاه)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        _timer = 0;
        fadeDir = true;
        isFadeTime = true;
    }


    public float GetHalfOfTransitionTime()
    {
        return 0.4f;
    }

    private void Update()
    {
        if (isFadeTime)
        {
            OnTransition?.Invoke(false);
            if (_timer > GetHalfOfTransitionTime() || _timer < 0)
            {
                isFadeTime = false;
                canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
                OnTransition?.Invoke(true);
               
            }
            else
            {
                _timer += fadeDir ? Time.deltaTime : (-Time.deltaTime);
                canvasGroup.alpha = (_timer / GetHalfOfTransitionTime());
            }
        }
    }
}