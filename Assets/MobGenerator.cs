using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobGenerator : MonoBehaviour {

    public GameObject[] Human;
    public GameObject[] Animal;
    public GameObject[] Car;
    public float[] mobxPos;
    public float[] carxPos;
    private int startpos = 0;
    private int stoppos = 160;
    private int generatepos = 100;
    public static bool generate = false;
    private List<GameObject> list_Human = new List<GameObject>();
    private List<GameObject> list_Animal = new List<GameObject>();
    private List<GameObject> list_Car = new List<GameObject>();
    private GameObject item_Human;
    private GameObject item_Animal;
    private GameObject item_Car; 


    // Use this for initialization
    void Start () {

        for (int i = 0; i < Human.Length; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                item_Human = Instantiate(Human[i]) as GameObject;
                item_Human.transform.position = new Vector3(-40, 0, -40);
                item_Human.SetActive(false);
                list_Human.Add(item_Human);
            }
        }

        for (int a = 0; a < Animal.Length; a++)
        {
            for (int b = 1; b <= 3; b++)
            {
                item_Animal = Instantiate(Animal[a]) as GameObject;
                item_Animal.transform.position = new Vector3(-40, 0, -40);
                item_Animal.SetActive(false);
                list_Animal.Add(item_Animal);
            }
        }

        for (int i = 0; i < Car.Length; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                item_Car = Instantiate(Car[i]) as GameObject;
                item_Car.transform.position = new Vector3(-40, 0, -40);
                item_Car.SetActive(false);
                list_Car.Add(item_Car);
            }
        }

        for (int i = startpos; i < stoppos; i += 10)
        {
            int item = Random.Range(1, 11);
            int mobxPosNum = Random.Range(0, 7);
            int carxPosNum = Random.Range(0, 4);
            int offsetZ = Random.Range(-5, 6);

            if (1 <= item && item <= 6)
            {
                int num = Random.Range(0, list_Human.Count);
                if(list_Human[num].gameObject.activeSelf == false)
                {
                    list_Human[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                    list_Human[num].SetActive(true);
                }              
            }
            else if (7 <= item && item <= 8)
            {
                int num = Random.Range(0, list_Animal.Count);
                if (list_Animal[num].gameObject.activeSelf == false)
                {
                    list_Animal[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                    list_Animal[num].SetActive(true);
                }
            }
            else if (9 <= item && item <= 10)
            {
                int num = Random.Range(0, list_Car.Count);
                if (list_Car[num].gameObject.activeSelf == false)
                {
                    list_Car[num].transform.position = new Vector3(carxPos[carxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                    list_Car[num].SetActive(true);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (generate)
        {
            int item = Random.Range(1, 11);
            int mobxPosNum = Random.Range(0, 7);
            int carxPosNum = Random.Range(0, 4);
            int offsetZ = Random.Range(-5, 6);
            if (1 <= item && item <= 6)
            {
                int num = Random.Range(0, list_Human.Count);
                if (list_Human[num].gameObject.activeSelf == false)
                {
                    list_Human[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, generatepos + offsetZ);
                    list_Human[num].SetActive(true);
                }else
                {
                    return;
                }
            }

            if (7 <= item && item <= 8)
            {
                int num = Random.Range(0, list_Animal.Count);
                if (list_Animal[num].gameObject.activeSelf == false)
                {
                    list_Animal[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, generatepos + offsetZ);
                    list_Animal[num].SetActive(true);
                }else
                {
                    return;
                }
            }

            if (9 <= item && item <= 10)
            {
                int num = Random.Range(0, list_Car.Count);
                if (list_Car[num].gameObject.activeSelf == false)
                {
                    list_Car[num].transform.position = new Vector3(carxPos[carxPosNum], list_Human[num].transform.position.y, generatepos + offsetZ);
                    list_Car[num].SetActive(true);
                }
                else
                {
                    return;
                }
            }

            generate = false;
        }
	
	}



}
