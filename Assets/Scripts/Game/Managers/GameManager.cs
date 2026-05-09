/*****************************************************************************
// File Name : GameManager.cs
// Author : Owen M. Friedlund
// Creation Date : February 12, 2026
//
// Brief Description : This is a document that holds the code for the GameManager class,
// which is used to manage the overall game state and logic
*****************************************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager gmInstance;
    private int score;
    [SerializeField] private int bolts;
    public int Bolts { get { return bolts; } set { bolts = value; } }
    private int scoreBonus;
    private GameObject howard;
    public GameObject spawnPoint;
    private GameObject startPoint;
    private GameObject GameOverScreen;
    private GameObject GameWinScreen;
    private GameObject menuScreen1;
    private GameObject menuScreen2;
    private GameObject menuScreen3;
    private GameObject tutorialScreen;
    public GameObject TutorialScreen { get { return tutorialScreen; } set { tutorialScreen = value; } }
    private bool pinFound = false;
    public bool PinFound { get { return pinFound; } set { pinFound = value; } }
    private bool keyFound = false;
    public bool KeyFound { get { return keyFound; } set { keyFound = value; } }
    private bool playerWon = false;
    public bool PlayerWon { get { return playerWon; } set { playerWon = value; } }
    [SerializeField] private TextMeshProUGUI airGaugeText;
    [SerializeField] private TextMeshProUGUI dullnessText;
    [SerializeField] private TextMeshProUGUI boltCounterText;
    [SerializeField] private TextMeshProUGUI spareTiresText;
    [SerializeField] private TextMeshProUGUI holeCounterText;
    [SerializeField] private GameObject boltPrefab;
    [SerializeField] private int lives;
    [SerializeField] private int startingLives;
    [SerializeField] private int sceneIndex;
    [SerializeField] private int menuIndex;
    public int MenuIndex { get { return menuIndex; } set { menuIndex = value; } }
    [SerializeField] private bool howardCanMove = true;
    [SerializeField] private float maxBoltDistance;
    [SerializeField] private float minBoltDistance;
    private int totalHoles;
    public int TotalHoles { get { return totalHoles; } set { totalHoles = value; } }
    public int[] levelHighScores = new int[5];
    private int levelIndex;
    public int LevelIndex { get { return levelIndex; } set { levelIndex = value; } }
    public bool HowardCanMove { get { return howardCanMove; } set { howardCanMove = value; } }
    private TextMeshProUGUI score1Text;
    private TextMeshProUGUI score2Text;
    private TextMeshProUGUI score3Text;
    private TextMeshProUGUI score4Text;
    private TextMeshProUGUI boltScoreText;
    private TextMeshProUGUI multiplierText;
    private TextMeshProUGUI bonusScoreText;
    private TextMeshProUGUI livesScoreText;
    private TextMeshProUGUI finalScoreText;
    private RawImage itemImageDisplay;
    private Texture itemImage;
    public Texture ItemImage { get { return itemImage; } set { itemImage = value; } }
    private TextMeshProUGUI itemNameText;
    public TextMeshProUGUI ItemNameText { get { return itemNameText; } set { itemNameText = value; } }
    private TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI ItemDescriptionText { get { return itemDescriptionText; } set { itemDescriptionText = value; } }
    /// <summary>
    /// This is the Awake function for the GameManager script, which initializes the singleton
    /// instance and sets up the game state
    /// It also subscribes to the sceneLoaded event to initialize the game state when a new scene is loaded
    /// </summary>
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

    /// <summary>
    /// This is the OnSceneLoaded function for the GameManager script,
    /// which initializes the game state when a new scene is loaded
    /// It sets up references to important game objects and UI elements, resets the player's position and rotation,
    /// and initializes the game state based on the scene that was loaded
    /// (e.g., showing the main menu or the level screens)
    /// It also resets the player's stats and sets the game over and win screens to inactive
    /// when a level scene is loaded, and it initializes the main menu screens and score displays
    /// when the main menu scene is loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        playerWon = false;
        Time.timeScale = 1;
        if (arg0.name != "Main Menu")
        {
            bolts = 0;
            howard = GameObject.FindGameObjectWithTag("Player");
            spawnPoint = GameObject.Find("SpawnPoint");
            startPoint = GameObject.Find("StartPoint");
            howard.transform.position = spawnPoint.transform.position;
            GameOverScreen = GameObject.Find("Game Over Screen");
            GameWinScreen = GameObject.Find("Win Screen");
            airGaugeText = GameObject.Find("Air Gauge").GetComponent<TextMeshProUGUI>();
            dullnessText = GameObject.Find("Dullness Meter").GetComponent<TextMeshProUGUI>();
            boltCounterText = GameObject.Find("Bolt Counter").GetComponent<TextMeshProUGUI>();
            spareTiresText = GameObject.Find("Lives").GetComponent<TextMeshProUGUI>();
            holeCounterText = GameObject.Find("Holes").GetComponent<TextMeshProUGUI>();
            boltScoreText = GameObject.Find("Bolt Score").GetComponent<TextMeshProUGUI>();
            multiplierText = GameObject.Find("Multiplier").GetComponent<TextMeshProUGUI>();
            bonusScoreText = GameObject.Find("Score Bonus").GetComponent<TextMeshProUGUI>();
            livesScoreText = GameObject.Find("Lives Remaining Score").GetComponent<TextMeshProUGUI>();
            finalScoreText = GameObject.Find("Final Score").GetComponent<TextMeshProUGUI>();
            GameOverScreen.SetActive(false);
            GameWinScreen.SetActive(false);
            boltScoreText.text = "";
            multiplierText.text = "";
            bonusScoreText.text = "";
            livesScoreText.text = "";
            finalScoreText.text = "";
            HowardCanMove = true;
            sceneIndex = arg0.buildIndex;
            if (arg0.name == "Tutorial")
            {
                tutorialScreen = GameObject.Find("How to Play");
                itemDescriptionText = GameObject.Find("Item Description").GetComponent<TextMeshProUGUI>();
                itemDescriptionText.text =
                    "Bolts are collectable items that increases your score when collected."
                    + "They can be found scattered around the level and have a chance to drop from jars.";
                itemNameText = GameObject.Find("Item Name").GetComponent<TextMeshProUGUI>();
                itemNameText.text = "Bolts";
                itemImageDisplay = GameObject.Find("Item Image").GetComponent<RawImage>();
                tutorialScreen.SetActive(true);
            }
        }
        else
        {
            menuScreen1 = GameObject.Find("Main");
            menuScreen2 = GameObject.Find("Level");
            menuScreen3 = GameObject.Find("How to Play");
            score1Text = GameObject.Find("Score 1").GetComponent<TextMeshProUGUI>();
            score2Text = GameObject.Find("Score 2").GetComponent<TextMeshProUGUI>();
            score3Text = GameObject.Find("Score 3").GetComponent<TextMeshProUGUI>();
            score4Text = GameObject.Find("Score 4").GetComponent<TextMeshProUGUI>();
            itemDescriptionText = GameObject.Find("Item Description").GetComponent<TextMeshProUGUI>();
            itemDescriptionText.text =
                "Bolts are collectable items that increases your score when collected. "
                + "They can be found scattered around the level and have a chance to drop from jars.";
            itemNameText = GameObject.Find("Item Name").GetComponent<TextMeshProUGUI>();
            itemNameText.text = "Bolts";
            itemImageDisplay = GameObject.Find("Item Image").GetComponent<RawImage>();
            menuScreen1.SetActive(true);
            menuScreen2.SetActive(false);
            menuScreen3.SetActive(false);
            score1Text.text = "";
            score2Text.text = "";
            score3Text.text = "";
            score4Text.text = "";
        }
    }
    /// <summary>
    /// Loads the game data from the json file
    /// </summary>
    void Start()
    {
        DataPersistenceManager.instance.LoadGame();
    }
    /// <summary>
    /// Updates the game state every frame, including the player's stats and the UI elements that display those stats
    /// It also checks if the player has won the game and if so, it calculates the final score
    /// and updates the win screen with the score breakdown and final score
    /// It also updates the main menu screens and score displays based on the menu index
    /// </summary>
    void Update()
    {
        if (howard != null)
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
            float tempFullnessMeter = (int)(playerFullness * 100);
            float tempDullnessMeter = (int)(playerDullness * 100);
            airGaugeText.text = "Air Fill - " + tempFullnessMeter / 100f + "%";
            boltCounterText.text = "Bolts - " + bolts;
            spareTiresText.text = "Spare Tires - " + lives;
            dullnessText.text = "Dullness - " + tempDullnessMeter / 100f + "%";
            holeCounterText.text = "Holes - " + playerHoles;
            if (playerWon)
            {
                Time.timeScale = 0;
                HowardCanMove = false;
                GameWinScreen.SetActive(true);
                if ((bolts * 1000 - totalHoles * 500) < 0)
                {
                    score = scoreBonus + (lives * 250);
                    boltScoreText.text = "Bolt Score [Bolts*1000 - Holes*500] - \n 0";
                }
                else
                {
                    score = (int)((bolts * 1000 - totalHoles * 500) * (1 + (100 - playerDullness) / 100f)) +
                        scoreBonus + (lives * 250);
                    boltScoreText.text = "Bolt Score [Bolts*1000 - Holes*500]: \n" + (bolts * 1000 - totalHoles * 500);
                }
                multiplierText.text = "Bolt/Hole Score Multiplier [1 + (100 - Dullness)/100]: \n" + (1 + (100 - playerDullness) / 100f);
                bonusScoreText.text = "Bonus Score: \n" + scoreBonus;
                livesScoreText.text = "Lives Remaining Score [Lives*250]: \n" + (lives * 250);
                finalScoreText.text = "Final Score: \n" + score;
                if (score > levelHighScores[sceneIndex])
                {
                    levelHighScores[sceneIndex] = score;
                }
            }
        }
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (menuIndex == 0)
            {
                menuScreen1.SetActive(true);
                menuScreen2.SetActive(false);
                menuScreen3.SetActive(false);
            }
            else if (menuIndex == 1)
            {
                menuScreen1.SetActive(false);
                menuScreen2.SetActive(true);
                menuScreen3.SetActive(false);
                score1Text.text = "Score: " + levelHighScores[1];
                score2Text.text = "Score: " + levelHighScores[2];
                score3Text.text = "Score: " + levelHighScores[3];
                score4Text.text = "Score: " + levelHighScores[4];
            }
            else if (menuIndex == 2)
            {
                menuScreen1.SetActive(false);
                menuScreen2.SetActive(false);
                menuScreen3.SetActive(true);
                itemImageDisplay.texture = itemImage;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            itemImageDisplay.texture = itemImage;
        }
    }
    /// <summary>
    /// This function is called when the player loses a life, and it updates the game state accordingly
    /// If the player still has lives remaining, it resets the player's position and rotation to the spawn point
    /// If the player has no lives remaining, it resets the player's stats, shows the game over screen,
    /// and resets the player's position and rotation to the start point
    /// </summary>
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
            GameObject.Find("Left Wheel").transform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
            GameObject.Find("Right Wheel").transform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
            GameOverScreen.SetActive(true);
            Time.timeScale = 0;
            spawnPoint.transform.position = startPoint.transform.position;
            spawnPoint.transform.rotation = startPoint.transform.rotation;
        }

    }
    /// <summary>
    /// Adds a life to the player's total lives, which can be used to continue playing after losing all lives
    /// </summary>
    public void AddLife()
    {
        lives++;
    }
    /// <summary>
    /// This function is called when the player chooses to retry after losing all lives,
    /// and it resets the game state to allow the player to try again
    /// It loads the current scene again, which resets all game objects and UI elements to their initial states,
    /// and it sets the HowardCanMove flag to true to allow the player to control Howard again
    /// </summary>
    public void Retry()
    {
        SceneManager.LoadScene(sceneIndex);
        HowardCanMove = true;
    }
    /// <summary>
    /// This function is a mechanic that spawns bolts around the player and sets the bolts count to 0
    /// It is called when the player collects a certain item that triggers this mechanic
    /// </summary>
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
                float z = howard.transform.position.z + Random.Range(minBoltDistance, maxBoltDistance);
                spawnBolts.Add(Instantiate(boltPrefab, new Vector3(x, y, z), Quaternion.identity));
            }
        }
        Bolts = 0;
    }
    /// <summary>
    /// This function is called to load the game data from the json file and it updates the game state based on that data
    /// </summary>
    /// <param name="data"></param>
    public void LoadData(GameData data)
    {
        this.levelIndex = data.lastLevelUnlocked;
        this.levelHighScores = data.levelHighScores;
    }
    /// <summary>
    /// This function is called to save the game data from all objects in the scene that implement the
    /// IDataPersistence interface and it saves that data to a file using the FileDataHandler class
    /// and if no game data is found, it initializes a new GameData object with default values and saves that instead
    /// </summary>
    public void SaveData(ref GameData data)
    {
        data.lastLevelUnlocked = this.levelIndex;
        data.levelHighScores = this.levelHighScores;
    }
}
