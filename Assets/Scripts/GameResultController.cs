using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameResultController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TitleButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadTitle", 0.8f);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void RestartButton()
    {
        this.GetComponent<AudioSource>().Play();
        Invoke("LoadRestart", 0.8f);
    }

    public void LoadRestart()
    {
        SceneManager.LoadScene("GameScene");
    }

}
