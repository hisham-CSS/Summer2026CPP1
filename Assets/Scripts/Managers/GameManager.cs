using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
    private static GameManager instance;
    public static GameManager Instance => instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    #region Lives
    public int maxLives = 9;
    private int _lives = 3;
    //C# style getters and setters - properties - they do the same thing as the above C++ style getters and setters, but they are more concise and easier to read - they are also more flexible, as they can have logic in them, and can be read-only or write-only
    public int Lives
    {
        get => _lives;
        set
        {
            if (value > maxLives)
            {
                maxLives = value;
            }
            else if (value < 0)
            {
                _lives = 0;
                //game over logic happens here
            }
            else if (value < _lives)
            {
                _lives = value;
                //respawn logic happens here
                Debug.Log("Respawn logic happens here");
            }
            else
            {
                _lives = value;
            }

            Debug.Log("Lives: " + _lives.ToString() + " Max Lives: " + maxLives.ToString());

        }
    }
    #endregion

    [SerializeField] private PlayerController playerPrefab;
    private PlayerController playerInstance;
    public PlayerController PlayerInstance => playerInstance;

    private Vector3 currentCheckpoint;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            string sceneToLoad = currentSceneName == "1.Title" ? "2.Game" : "1.Title";

            SceneManager.LoadScene(sceneToLoad);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Lives++;
        }
    }

    public void SpawnPlayer(Vector3 pos)
    {
        playerInstance = Instantiate(playerPrefab, pos, Quaternion.identity);
        UpdateCheckpoint(pos);
    }

    public void UpdateCheckpoint(Vector3 newPos)
    {
        currentCheckpoint = newPos;
    }
}
