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
    void Start()
    {
        numContents = Random.Range(1, numContentsMax + 1);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject jarContents;
            if (Random.Range(0f, 1f) == 0) // 50% chance to spawn nails or bolts
            {
                jarContents = nailPrefab;
            }
            else
            {
                jarContents = boltPrefab;
            }
            for (int i = 0; i < numContents; i++)
            {
                float x = transform.position.x + Random.Range(minBoltDistance/2, maxBoltDistance/2);
                float y = transform.position.y + Random.Range(0, maxBoltDistance/2);
                float z = transform.position.z + Random.Range(minBoltDistance/2, maxBoltDistance/2);
                Instantiate(jarContents, new Vector3(x, y, z), Quaternion.identity);
            }
            for (int i = 0; i < Random.Range(1, 4); i++)
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
