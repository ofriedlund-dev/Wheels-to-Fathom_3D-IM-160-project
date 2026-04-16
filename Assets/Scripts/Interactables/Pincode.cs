using UnityEngine;

public class Pincode : BaseInteractable
{
    public Pincode() : base(true) { }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.gmInstance.PinFound = true;
            Use();
        }
    }
}
