using UnityEngine;

public class LevelMenu : MonoBehaviour
{
    public void SwitchMenu(int menuIndex)
    {
        GameManager.gmInstance.MenuIndex = menuIndex;
    }
}
