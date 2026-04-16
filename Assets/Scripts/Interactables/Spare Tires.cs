using UnityEngine;

public class SpareTires : BaseInteractable
{
    public SpareTires() : base(true) { }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.gmInstance.AddLife();
            Use();
        }
    }
}
