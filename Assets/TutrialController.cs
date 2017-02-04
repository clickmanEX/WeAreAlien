using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutrialController : MonoBehaviour {

    private GameObject page1;
    private GameObject page2;
    private GameObject next;
    private GameObject back;


    // Use this for initialization
    void Start () {

        this.page1 = GameObject.Find("Page1");
        this.page2 = GameObject.Find("Page2");
        this.page2.gameObject.SetActive(false);
        this.next = GameObject.Find("NextPageButton");
        this.back = GameObject.Find("BackPageButton");
        this.back.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
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

    public void NextPageButton()
    {
        this.GetComponent<AudioSource>().Play();
        this.page1.gameObject.SetActive(false);
        this.page2.gameObject.SetActive(true);
        this.next.gameObject.SetActive(false);
        this.back.gameObject.SetActive(true);
    }

    public void BackPageButton()
    {
        this.GetComponent<AudioSource>().Play();
        this.page1.gameObject.SetActive(true);
        this.page2.gameObject.SetActive(false);
        this.next.gameObject.SetActive(true);
        this.back.gameObject.SetActive(false);
    }

}
