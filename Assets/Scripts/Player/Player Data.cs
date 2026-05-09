/*****************************************************************************
// File Name : Player Data.cs
// Author : Owen M. Friedlund
// Creation Date : February 12, 2026
//
// Brief Description : This is a document that holds the code for the PlayerData class,
// which is used to store the player's stats and information
*****************************************************************************/
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float dullness = 0;
    public float Dullness {
        get {return dullness;} 
        set
        {
            if (value <= 100 && value >= 0)
            {
                dullness = value;
            }
            else if (value > 100)
            {
                dullness = 100;
            }
            else
            {
                dullness = 0;
            }
            
        }
    }
    private float fullness = 100;
    public float Fullness {
        get {return fullness;} 
        set
        {
            if (value <= 100 && value >= 0)
            {
                fullness = value;
            }
            else if (value > 100)
            {
                fullness = 100;
            }
            else
            {
                fullness = 0;
            }
            
        }
    }
    private bool punctured;
    public bool Punctured {
        get {return punctured;}
        set
        {
            punctured = value;
        }
    }
    private int holes;
    public int Holes {
        get {return holes;} 
        set
        {
            if (!punctured)
            {
                holes = 0;
            }
            else
            {
                holes = value;
                if (value == 0) punctured = false;
            }
        }
    }
    /// <summary>
    /// This is a function that displays the player's stats in a readable format
    /// </summary>
    /// <returns></returns>
    public string DisplayPlayerInfo()
    {
        return $"Dullness: {Dullness} \nFullness: {Fullness}\nPunctured: {Punctured}\nHoles: {Holes}";
    }
    /// <summary>
    /// This is a function that resets the player's stats to their default values
    /// </summary>
    public void ResetData()
    {
        Dullness = 0;
        Fullness = 100;
        Punctured = false;
        Holes = 0;
    }
}
