using UnityEngine;

public class Bolts : BaseInteractable
{
    private bool collected = false;
    public Bolts() : base(true) {}
    public Bolts(bool dropped) : base(true) {}
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Use();
        }
    }
    protected override void Use()
    {
        if (OneUse && !collected)
        {
            GameManager.gmInstance.Bolts++;
            collected = true;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
