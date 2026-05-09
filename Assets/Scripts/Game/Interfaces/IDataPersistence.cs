/*****************************************************************************
// File Name : IDataPersistance.cs
// Author : Owen M. Friedlund
// Creation Date : April 17, 2026
//
// Brief Description : This is a document that holds the base functions for loading and saving data
*****************************************************************************/
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
