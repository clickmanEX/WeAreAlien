using UnityEngine;
using System.Collections;

public class MobGenerator : MonoBehaviour {

    public GameObject[] Human;
    public GameObject[] Animal;
    public GameObject[] Car;
    public float[] mobxPos;
    public float[] carxPos;
    private int startpos = 0;
    private int stoppos = 150;
    private int generatepos = 100;
    public static bool generate = false;

    // Use this for initialization
    void Start () {
        for (int i = startpos; i < stoppos; i += 10)
        {
            int item = Random.Range(1, 11);
            int mobxPosNum = Random.Range(0, 7);
            int carxPosNum = Random.Range(0, 4);
            int offsetZ = Random.Range(-5, 6);
            if (1 <= item && item <= 6)
            {
                int num = Random.Range(0, 10);
                GameObject human = Instantiate(Human[num]) as GameObject;
                human.transform.position = new Vector3(mobxPos[mobxPosNum], human.transform.position.y, i + offsetZ);
            }
            else if (7 <= item && item <= 8)
            {
                int num = Random.Range(0, 5);
                GameObject animal = Instantiate(Animal[num]) as GameObject;
                animal.transform.position = new Vector3(mobxPos[mobxPosNum], animal.transform.position.y, i + offsetZ);
            }
            else if (9 <= item && item <= 10)
            {
                int num = Random.Range(0, 5);
                GameObject car = Instantiate(Car[num]) as GameObject;
                car.transform.position = new Vector3(carxPos[carxPosNum], car.transform.position.y, i + offsetZ);
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
                int num = Random.Range(0, 10);
                GameObject human = Instantiate(Human[num]) as GameObject;
                human.transform.position = new Vector3(mobxPos[mobxPosNum], human.transform.position.y, generatepos + offsetZ);
            }
            else if (7 <= item && item <= 8)
            {
                int num = Random.Range(0, 5);
                GameObject animal = Instantiate(Animal[num]) as GameObject;
                animal.transform.position = new Vector3(mobxPos[mobxPosNum], animal.transform.position.y, generatepos + offsetZ);
            }
            else if (9 <= item && item <= 10)
            {
                int num = Random.Range(0, 5);
                GameObject car = Instantiate(Car[num]) as GameObject;
                car.transform.position = new Vector3(carxPos[carxPosNum], car.transform.position.y, generatepos + offsetZ);
            }

            generate = false;
        }
	
	}
}
