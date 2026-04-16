using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated;
    public bool IsActivated { get { return isActivated; } set { isActivated = value; } }
    void Start()
    {
        isActivated = false;
        GetComponent<Light>().color = Color.yellow; // Set the checkpoint's light color to red when not activated
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = true;
            Debug.Log("Checkpoint activated!");
            GameManager.gmInstance.spawnPoint.transform.position = transform.position;
            GetComponent<Light>().color = Color.green; // Change the checkpoint's light color to green when activated
        }
    }
}
