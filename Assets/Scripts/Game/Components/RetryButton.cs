using UnityEngine;

public class RetryButton : MonoBehaviour
{
    public void RetryFunction()
    {
        if (GameManager.gmInstance != null)
        {
            GameManager.gmInstance.Retry();
        }
    }
}
