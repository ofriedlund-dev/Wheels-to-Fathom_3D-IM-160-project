/*****************************************************************************
// File Name : Jar.cs
// Author : Owen M. Friedlund
// Creation Date : April 7, 2026
//
// Brief Description : This is a document that holds the code for the Jar class,
// which represents the jar that the player can interact with in the game
*****************************************************************************/
using UnityEngine;

public class Jar : BaseInteractable
{
    [SerializeField] private GameObject nailPrefab;
    [SerializeField] private GameObject boltPrefab;
    [SerializeField] private GameObject glassShardPrefab;
    [SerializeField] private int numContentsMax;
    [SerializeField] private float maxBoltDistance;
    [SerializeField] private float minBoltDistance;
    private int numContents;
    public Jar() : base(true) { }
    /// <summary>
    /// sets the number of contents in the jar to a random value between 1 and numContentsMax when the game starts
    /// </summary>
    void Start()
    {
        numContents = Random.Range(1, numContentsMax + 1);
    }
    /// <summary>
    /// This function is called when the player collides with the jar object,
    /// and it checks if the colliding object is the player,
    /// and if it is, it randomly decides whether to spawn nails or bolts from the jar
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject jarContents;
            if (Random.Range(0f, 1f) < 0.25f) // 25% chance to spawn nails or bolts
            {
                jarContents = nailPrefab;
                numContents = (numContents + 1) / 2; // If it's nails, spawn fewer of them
            }
            else
            {
                jarContents = boltPrefab;
                numContents = numContents * 2; // If it's bolts, spawn more of them
            }
            for (int i = 0; i < numContents; i++)
            {
                float x = transform.position.x + Random.Range(minBoltDistance/2, maxBoltDistance/2);
                float y = transform.position.y + Random.Range(0, maxBoltDistance/2);
                float z = transform.position.z + Random.Range(minBoltDistance/2, maxBoltDistance/2);
                Instantiate(jarContents, new Vector3(x, y, z), Quaternion.identity);
            }
            for (int i = 0; i < Random.Range(10, 21); i++)
            {
                float x = transform.position.x + Random.Range(minBoltDistance, maxBoltDistance);
                float y = transform.position.y + Random.Range(0, maxBoltDistance);
                float z = transform.position.z + Random.Range(minBoltDistance, maxBoltDistance);
                Instantiate(glassShardPrefab, new Vector3(x, y, z), Quaternion.identity);
            }
            Use();
        }
    }
}
