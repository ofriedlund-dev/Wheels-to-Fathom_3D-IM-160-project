/*****************************************************************************
// File Name : GameData.cs
// Author : Owen M. Friedlund
// Creation Date : April 17, 2026
//
// Brief Description : This is a document that defines what data is saved into the json file
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int lastLevelUnlocked;
    public int[] levelHighScores = new int[5];
    public GameData()
    {
        lastLevelUnlocked = 0;
        for (int i = 0; i < levelHighScores.Length; i++)
        {
            levelHighScores[i] = 0;
        }
    }
}
