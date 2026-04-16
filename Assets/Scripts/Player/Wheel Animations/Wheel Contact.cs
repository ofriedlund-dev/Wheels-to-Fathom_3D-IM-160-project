using UnityEngine;

public class WheelContact : MonoBehaviour
{
    private PlayerController playerController;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController = GetComponentInParent<PlayerController>();
            playerController.Grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerController = GetComponentInParent<PlayerController>();
            playerController.Grounded = false;
        }
    }
}
