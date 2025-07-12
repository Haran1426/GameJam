using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] GameObject setting; //설정창
    // Start is called before the first frame update
    void Start()
    {
        setting.SetActive(false);
    }
    public void gameStart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void Setting()
    {
        setting.SetActive(true);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    public void X()
    {
        setting.SetActive(false);
    }
}
