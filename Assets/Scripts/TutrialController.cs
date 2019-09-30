using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutrialController : MonoBehaviour
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

}
