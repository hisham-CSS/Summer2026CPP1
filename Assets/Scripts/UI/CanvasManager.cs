using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button backButton;

    [Header("In Gamee UI")]
    [SerializeField] private TMP_Text livesText;

    [Header("Menu Referendces")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => ChangeScene("2.Game"));
        
        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenu(settingsMenu, mainMenu));
        
        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenu(null, pauseMenu));

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => ChangeScene("1.Title"));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenu(mainMenu, settingsMenu));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void QuitGame()
    {
        #if UNITY_EDITOR     
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    void SetMenu(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate != null) menuToActivate.SetActive(true);
        if (menuToDeactivate != null) menuToDeactivate.SetActive(false);
    }    
}
