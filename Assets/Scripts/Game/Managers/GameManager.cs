using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gmInstance;
    private int score;
    [SerializeField] private int bolts;
    public int Bolts { get{return bolts;} set{bolts = value;} }
    private int scoreBonus;
    private GameObject howard;
    public GameObject spawnPoint;
    private GameObject startPoint;
    private GameObject GameOverScreen;
    [SerializeField] private TextMeshProUGUI airGaugeText;
    [SerializeField] private TextMeshProUGUI dullnessText;
    [SerializeField] private TextMeshProUGUI boltCounterText;
    [SerializeField] private TextMeshProUGUI spareTiresText;
    [SerializeField] private TextMeshProUGUI holeCounterText;
    [SerializeField] private GameObject boltPrefab;
    [SerializeField] private int lives;
    [SerializeField] private int startingLives;
    [SerializeField] private int sceneIndex;
    [SerializeField] private bool howardCanMove = true;
    [SerializeField] private float maxBoltDistance;
    [SerializeField] private float minBoltDistance;

    public bool HowardCanMove { get{return howardCanMove;} set{howardCanMove = value;} }
    void Awake()
    {
        if (gmInstance == null)
        {
            gmInstance = this;
            DontDestroyOnLoad(gmInstance.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (gmInstance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != "Main Menu")
        {
            howard = GameObject.FindGameObjectWithTag("Player");
            spawnPoint = GameObject.Find("SpawnPoint");
            startPoint = GameObject.Find("StartPoint");
            howard.transform.position = spawnPoint.transform.position;
            GameOverScreen = GameObject.Find("Game Over Screen");
            airGaugeText = GameObject.Find("Air Gauge").GetComponent<TextMeshProUGUI>();
            dullnessText = GameObject.Find("Dullness Meter").GetComponent<TextMeshProUGUI>();
            boltCounterText = GameObject.Find("Bolt Counter").GetComponent<TextMeshProUGUI>();
            spareTiresText = GameObject.Find("Lives").GetComponent<TextMeshProUGUI>();
            holeCounterText = GameObject.Find("Holes").GetComponent<TextMeshProUGUI>();
            GameOverScreen.SetActive(false);
            HowardCanMove = true;
            sceneIndex = arg0.buildIndex;
        }
    }

    void Update()
    {
        float playerDullness = howard.GetComponent<PlayerData>().Dullness;
        float playerFullness = howard.GetComponent<PlayerData>().Fullness;
        float playerHoles = howard.GetComponent<PlayerData>().Holes;
        if (playerDullness <= 25)
        {
            scoreBonus = 10000;
        }
        else if (playerDullness <= 50)
        {
            scoreBonus = 7500;
        }
        else if (playerDullness <= 75)
        {
            scoreBonus = 5000;
        }
        else
        {
            scoreBonus = 2500;
        }
        airGaugeText.text = "Air Fill - " + playerFullness + "%";
        boltCounterText.text = "Bolts - " + bolts;
        spareTiresText.text = "Spare Tires - " + lives;
        dullnessText.text = "Dullness - " + playerDullness + "%";
        holeCounterText.text = "Holes - " + playerHoles;
    }
    public void LoseLife()
    {
        lives--;
        if (lives > 0)
        {
            howard.transform.position = spawnPoint.transform.position;
            howard.transform.rotation = spawnPoint.transform.rotation;
        }
        else
        {
            lives = startingLives;
            score = 0;
            scoreBonus = 0;
            howard.GetComponent<PlayerData>().ResetData();
            HowardCanMove = false;
            GameOverScreen.SetActive(true);
            spawnPoint.transform.position = startPoint.transform.position;
            spawnPoint.transform.rotation = startPoint.transform.rotation;
        }
        
    }
    public void Retry()
    {
        SceneManager.LoadScene(sceneIndex);
        HowardCanMove = true;
    }
    public void TotallyNotSonicRingsMechanic()
    {
        List<GameObject> spawnBolts = new List<GameObject>();
        int spawnCount = Bolts;
        if (spawnBolts.Count < spawnCount)
        {
            for (int i = spawnCount; spawnBolts.Count < i;)
            {
                float x = howard.transform.position.x + Random.Range(minBoltDistance, maxBoltDistance);
                float y = howard.transform.position.y + Random.Range(0, maxBoltDistance);
                float z = howard.transform.position.z +Random.Range(minBoltDistance, maxBoltDistance);
                spawnBolts.Add(Instantiate(boltPrefab, new Vector3(x,y,z), Quaternion.identity));
            }
        }
        Bolts = 0;
    }
}
