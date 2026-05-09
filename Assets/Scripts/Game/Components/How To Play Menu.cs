/*****************************************************************************
// File Name : How To Play Menu.cs
// Author : Owen M. Friedlund
// Creation Date : April 26, 2026
//
// Brief Description : This is a document that provides the text and functions for the how to menu
*****************************************************************************/
using UnityEngine;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField] private Texture[] itemImages;
    private string[] itemNames = {
        "Bolts",
        "Nails",
        "Jars",
        "Plugs",
        "Pumps", 
        "Spare Tires",
        "Wheel Jacks", 
        "Pin Code", 
        "Garage Door" 
        }; 
    private string[] itemDescriptions = {
        "Bolts are collectable items that increases your score when collected. " +
            "They can be found scattered around the level and have a chance to drop from jars. ",
        "Nails are hazardous items that can add a hole into your tires and cause you to leak air. " + 
            "They can be found scattered around the level and have a chance to drop from jars as well. ",
        "Jars are scattered around the level and can be broken by driving into them. " +
            "They have a chance to drop Bolts, Nails. " +
            "Beware though, they drop glass shards as well that can damage your tires and cause you to leak air. ",
        "Plugs are collectable items that patch holes in your tires and prevent you from leaking air. " + 
            "They can be found scattered around the level. ",
        "Pumps are collectable items that inflate your tires. " + 
            "They can be found scattered around the level. " + 
            "Beware though, overinflating your tires can cause them to pop. ",
        "Spare Tires are collectable items that give you extra lives. They can be found scattered around the level. ",
        "Wheel Jacks can launch you into the air when you drive into them and use the launch control. " + 
            "They can be found scattered around the level and help you avoid obstacles and more around. ",
        "The Pin Code is a collectable item that unlocks the garage door so you can finish the level. " + 
            "It is found in one place in the level and is required to finish the level. ",
        "The Garage Door is the exit of the level. " + 
            "It is locked at the start of the level and can only be unlocked by collecting the Pin Code. " + 
            "Once you have unlocked it, drive into it to finish the level. "
    };
    private int currentIndex = 0;
    /// <summary>
    /// Shows the info on the menu based on the item selected
    /// </summary>
    /// <param name="index"></param>
    public void ShowItemInfo(int index)
    {
        if (index >= 0 && index < itemNames.Length)
        {
            currentIndex = index;
            GameManager.gmInstance.ItemImage = itemImages[index];
            GameManager.gmInstance.ItemNameText.text = itemNames[index];
            GameManager.gmInstance.ItemDescriptionText.text = itemDescriptions[index];
        }
    }
    /// <summary>
    /// Moves to the next item in the list
    /// </summary>
    public void NextItem()
    {
        currentIndex++;
        if (currentIndex >= itemNames.Length)
        {
            currentIndex = 0;
        }
        ShowItemInfo(currentIndex);
    }
    /// <summary>
    /// Moves to the prior item on the list
    /// </summary>
    public void PreviousItem()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = itemNames.Length - 1;
        }
        ShowItemInfo(currentIndex);
    }
}
