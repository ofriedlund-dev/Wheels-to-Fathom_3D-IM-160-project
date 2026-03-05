using Unity.VisualScripting;
using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
    private bool oneUse = true;
    public bool OneUse { get { return oneUse; } set { oneUse = value; } }
    public BaseInteractable() { }
    public BaseInteractable(bool value) { oneUse = value; }
    protected virtual void Use()
    {
        if (oneUse)
        {
            Destroy(this.gameObject);
        }
    }
}
