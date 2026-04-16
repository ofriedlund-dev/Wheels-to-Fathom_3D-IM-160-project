using UnityEngine;

public class Nail : BaseInteractable
{
    public Nail() : base(true) { }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
}
