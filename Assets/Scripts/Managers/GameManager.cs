using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
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
    [Range(0, 9)]
    public int startingLives = 3;
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
                GameOver();
            }
            else if (value < _lives)
            {
                _lives = value;
                Respawn();
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


    //event driven programming: paradigm in which the flow of the program is determined by events such has user input rather than a linear sequence of instructions.  Allows for more flexibility and responsiveness in a program as it can react to event as they happen rather than waiting for a specific point in code to execute.  Events are the backbone of the observer pattern.

    //Observer pattern: The Subject (object) maintains a list of dependants will notify them of any specfic event they may need to listen to. This can decouple the caller from the people requiring it as the subject does not need to know about what its dependants are doing.

    //delegates are a type that represents a method or a funciton with a specific parameter list and return type.
    public delegate void PlayerInstanceDelegate(PlayerController player);
    public event PlayerInstanceDelegate OnPlayerSpawned;

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

        if (Input.GetKeyDown(KeyCode.K))
        {
            Lives--;
        }
    }

    public void SpawnPlayer(Vector3 pos)
    {
        Lives = startingLives;

        playerInstance = Instantiate(playerPrefab, pos, Quaternion.identity);
        OnPlayerSpawned?.Invoke(playerInstance);
        UpdateCheckpoint(pos);
    }

    public void UpdateCheckpoint(Vector3 newPos)
    {
        currentCheckpoint = newPos;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("1.Title");
    }

    private void Respawn()
    {
        //this could play an animation and reload the level or do something more elaborate if you needed it to.
        playerInstance.transform.position = currentCheckpoint;
    }
}
