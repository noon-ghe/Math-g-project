// Change Default scripts on Editor\Data\Resources\ScriptTemplates

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]private AudioClip clickSound;
    
    
    static SoundManager _i; //  _i ←→ _instance 

    public static SoundManager I
    {
        //  _i ←→ Instance 
        get
        {
            //Singleton
            if (_i == null) //Are we have SoundManager before? Checking...
            {
                _i = FindObjectOfType<SoundManager>();
                if (_i == null)
                {
                    //Ok... we dont have any <SoundManager>(); then create that
                    GameObject mySoundManager = new GameObject("SoundManager");
                    mySoundManager.AddComponent<SoundManager>();
                    _i = mySoundManager.GetComponent<SoundManager>();
                }
            }

            //return _i anyway
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null) // if we have _i (SoundManager) before, then destroy me bos...
            Destroy(this);
        DontDestroyOnLoad(this.gameObject); //it's ok... i'm first <SoundManager>() now.
    }

    public void PlayClickSound()
    {
        GetComponent<AudioSource>().PlayOneShot(clickSound);
    }
}   