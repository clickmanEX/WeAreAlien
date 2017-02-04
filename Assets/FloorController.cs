using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour
{

    public float scrollSpeed;           //床のスクロールスピード
    private float deadLine = -100f;     //床が非表示される位置
    public FloorController floor;       //該当するfloorの一つ後ろのものを参照。例)Floor1(3)ならFloor1(2)、Floor1(1)ならFloor1(3)

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //床を再配置する時、ぴったり後ろに配置されるように処理
        if (transform.position.z < this.deadLine)
        {
            transform.position = new Vector3(0, 0, floor.transform.position.z + 100);
        }

        if (LifeController.isEnd)
        {
            this.scrollSpeed *= 0.99f;
        }

        if (UFOController.isBoostButtonDown)
        {
            transform.Translate(0, Time.deltaTime * this.scrollSpeed * 3, 0);
        }
        else
        {
            transform.Translate(0, Time.deltaTime*this.scrollSpeed, 0);
        }

        

    }
}
