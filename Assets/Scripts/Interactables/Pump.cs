using UnityEngine;

public class Pump : BaseInteractable
{
    public Pump() : base(true) { }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
}
