using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Riddle", fileName = "New Riddle")]
public class RiddleSO : ScriptableObject
{
    [SerializeField] private Sprite riddleSprite; //Sprite of the each level 
    [SerializeField] private string correctAnswere;
    [SerializeField] private string[] soloutions = new string[3];

    public Sprite GetRiddleSprite()
    {
        return riddleSprite;
    }

    public string GetAnswere()
    {
        return correctAnswere;
    }

    public string GetSoloution(int index)
    {
        return soloutions[index];
    }
}