using UnityEngine;

public class Garage : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && GameManager.gmInstance.PinFound)
        {
            Debug.Log("Garage Door Opened");
            GameManager.gmInstance.PlayerWon = true;
        }
    }
}
