using UnityEngine;

public class Nail : BaseInteractable
{
    public Nail() : base(true) { }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
}
