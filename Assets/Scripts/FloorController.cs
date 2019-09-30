using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour
{
    [SerializeField] Transform prevFloorTrs;
    public float scrollSpeed;
    private float deadLine = -100f;

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
            transform.position = new Vector3(0, 0, prevFloorTrs.position.z + 100);
        }

        if (GameManager.Instance.IsGameEnd())
        {
            this.scrollSpeed *= 0.99f;
        }


        if (UFOController.isBoostButtonDown)
        {
            transform.Translate(0, Time.deltaTime * this.scrollSpeed * 3, 0);
        }
        else
        {
            transform.Translate(0, Time.deltaTime * this.scrollSpeed, 0);
        }
    }
}
