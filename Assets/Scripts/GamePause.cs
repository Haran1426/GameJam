using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI; //게임퍼즈 팝업창
    [SerializeField] GameObject background; //뒷 배경
    [SerializeField] GameObject Setting; //설정창

    private bool isPause = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        background.SetActive(false);
        Setting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if(isPause)
                ContinueGame();
            else
                PauseGame();
        }
    }
    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        background.SetActive(true);
    }
    public void ContinueGame()
    {
        isPause = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        background.SetActive(false);
    }
    public void OpenSetting()
    {
        Setting.SetActive(true);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void CloseSetting()
    {
        Setting.SetActive(false);
    }
}
