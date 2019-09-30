using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobGenerator : MonoBehaviour
{

    public GameObject[] Human;      //人間系モブキャラのプレハブを入れる配列
    public GameObject[] Animal;     //動物系モブキャラのプレハブを入れる配列
    public GameObject[] Car;        //乗り物系モブキャラのプレハブを入れる配列
    public float[] mobxPos;         //人間・動物系モブキャラが出現するx軸の値を入れた配列
    public float[] carxPos;         //乗り物系モブキャラが出現するx軸の値を入れた配列
    private int startpos = -50;     //最初に配置されるモブキャラのz軸の最初の値
    private int stoppos = 210;      //最初に配置されるモブキャラのz軸の最後の値
    private int generatepos = 150;  //ゲーム中に配置されるモブキャラのz軸の値
    public static int generateCount = 0;    //配置されたモブキャラをカウントするint型整数。MobControllerに使用。
    private List<GameObject> list_Human = new List<GameObject>();   //最初に生成した人間系モブキャラを入れるためのList
    private List<GameObject> list_Animal = new List<GameObject>();  //最初に生成した動物系モブキャラを入れるためのList
    private List<GameObject> list_Car = new List<GameObject>();     //最初に生成した乗り物系モブキャラを入れるためのList
    private GameObject item_Human;      //生成したモブキャラの収納先
    private GameObject item_Animal;     //生成したモブキャラの収納先
    private GameObject item_Car;        //生成したモブキャラの収納先


    // Use this for initialization
    void Start()
    {
        generateCount = 0;

        //オブジェクトプールの作成
        //配列のi番目に収納されているプレハブを3つ生成して非表示にし、Listに入れる
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

        //配列のi番目に収納されているプレハブを3つ生成して非表示にし、Listに入れる
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

        //配列のi番目に収納されているプレハブを3つ生成して非表示にし、Listに入れる
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

        //ゲームスタート時のモブキャラの配置
        //startposからstopposまで10刻みでモブキャラを配置していく。
        for (int i = startpos; i < stoppos; i += 10)
        {
            generateCount++;
            int item = Random.Range(1, 11);
            int mobxPosNum = Random.Range(0, 7);
            int carxPosNum = Random.Range(0, 4);
            int offsetZ = Random.Range(-5, 6);      //等間隔で配置されないようにz軸方向にランダム数値を入れる。

            if (1 <= item && item <= 6)
            {
                while(true)
                {
                    //ランダム数値のnumより、list_Human[num]のactiveSelfがfalseだった場合、trueにして配置
                    int num = Random.Range(0, list_Human.Count);
                    if (list_Human[num].gameObject.activeSelf == false)
                    {
                        list_Human[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                        list_Human[num].SetActive(true);
                        break;
                    }
                    else
                    {
                        continue; //ランダム数値のnumより、list_Human[num]のactiveSelfがtureだった場合、再抽選する
                    }
                }
                
            }
            else if (7 <= item && item <= 8)
            {
                while (true)
                {
                    //ランダム数値のnumより、list_Animal[num]のactiveSelfがfalseだった場合、trueにして配置
                    int num = Random.Range(0, list_Animal.Count);
                    if (list_Animal[num].gameObject.activeSelf == false)
                    {
                        list_Animal[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                        list_Animal[num].SetActive(true);
                        break;
                    }
                    else
                    {
                        continue; //ランダム数値のnumより、list_Animal[num]のactiveSelfがtureだった場合、再抽選する
                    }
                }
                
            }
            else if (9 <= item && item <= 10)
            {
                while (true)
                {
                    //ランダム数値のnumより、list_Car[num]のactiveSelfがfalseだった場合、trueにして配置
                    int num = Random.Range(0, list_Car.Count);
                    if (list_Car[num].gameObject.activeSelf == false)
                    {
                        list_Car[num].transform.position = new Vector3(carxPos[carxPosNum], list_Human[num].transform.position.y, i + offsetZ);
                        list_Car[num].SetActive(true);
                        break;
                    }
                    else
                    {
                        continue; //ランダム数値のnumより、list_Car[num]のactiveSelfがtureだった場合、再抽選する
                    }
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //モブキャラが吸い取られた、画面外に行った場合すぐ補充するようにする処理
        if (generateCount <= 25)
        {
            int item = Random.Range(1, 11);
            int mobxPosNum = Random.Range(0, 7);
            int carxPosNum = Random.Range(0, 4);
            int offsetZ = Random.Range(-10, 10);
            if (1 <= item && item <= 6)
            {
                int num = Random.Range(0, list_Human.Count);
                if (list_Human[num].gameObject.activeSelf == false)
                {
                    list_Human[num].transform.position = new Vector3(mobxPos[mobxPosNum], list_Human[num].transform.position.y, generatepos + offsetZ);
                    list_Human[num].SetActive(true);
                }
                else        //ランダム数値のnumより、list_Human[num]のactiveSelfがtureだった場合、この処理を中断して再抽選する。
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
                }
                else
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

            generateCount++;
        }

    }



}