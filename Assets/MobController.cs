using UnityEngine;
using System.Collections;

public class MobController : MonoBehaviour
{

    private Rigidbody mobRigidbody;
    private GameObject ufo;             //UFOタグを付けたオブジェクトの変数
    private float movetime = 1.5f;      //ある距離をこの時間かけて動かしたいとき
    public float forwardForce = -10f;   //基本血：-10f　車などの早く動かしたいものはインスペクタで別途調整
    public float upForce = 300f;        //基本値：300f　車などの跳ねさせたくないものはインスペクタで0に調整
    private float upDistance;           //モブキャラの上昇距離
    private float upSpeed;              //モブキャラの上昇速度              
    private float rotatespeed = 2f;     //モブキャラの回転速度
    private float rotatereturn = 0f;    //モブキャラの向きを直すための変数
    private bool uping = false;         //モブキャラが吸い取られている状態かを表すブール変数
    private bool jumping = false;       //モブキャラが上昇中を表すブール変数
    private bool dropDown = false;      //モブキャラが落ちている最中かを表すブール変数

    // Use this for initialization
    void Start()
    {
        this.mobRigidbody = GetComponent<Rigidbody>();
        this.ufo = GameObject.Find("UFO");
        upDistance = ufo.transform.position.y;
        upSpeed = upDistance / movetime;

    }

    // Update is called once per frame
    void Update()
    {
        //落下中じゃない時はforwardForce値の速さで移動、ブーストボタン押してる最中は10倍速
        if (this.dropDown == false)
        {
            if (UFOController.isBoostButtonDown)
            {
                this.transform.Translate(0, 0, Time.deltaTime * forwardForce*10);
            }
            else
            {
                this.transform.Translate(0, 0, Time.deltaTime * forwardForce);
            }
        }
        else if (this.dropDown)         //落下中は垂直落下するようにする。
        {
            this.transform.Translate(0, 0, 0);
        }
        

        if (this.jumping)
        {
            this.mobRigidbody.AddForce(this.transform.up * this.upForce);
            this.jumping = false;
        }

        //UFOに吸い取られている時、物理演算を受けないようにし、モブキャラが回転した値を保存する。
        if (this.uping)
        {
            this.rotatereturn += rotatespeed;
            this.transform.Rotate(0, rotatespeed, 0);
            this.mobRigidbody.isKinematic = true;
            this.transform.Translate(0, Time.deltaTime * upSpeed, 0);
            this.dropDown = true;
        }
        else
        {
            this.mobRigidbody.isKinematic = false;

        }

        //万が一、DeadLineをすり抜けてしまったときの回収処理。
        if (LifeController.isEnd == false)
        {
            if (this.transform.position.y < -100)
            {
                MobGenerator.generateCount--;
                this.gameObject.SetActive(false);
            }
        }
        

    }

    //UFOの光に入ったときにキャッチボタンを押すと、UFOの中心地にモブキャラが移動して吸い込まれる動作をする。
    void OnTriggerStay(Collider triggerStay)
    {
        this.uping = false;

        if (Input.GetKey(KeyCode.Space) || UFOController.isCatchButtonDown)
        {
            this.uping = true;
            this.transform.position = new Vector3(ufo.transform.position.x, this.transform.position.y, ufo.transform.position.z);

        }

    }

    //UFOや床に衝突したときの処理
    void OnCollisionEnter(Collision other)
    {
        //UFOに衝突したら見えないようにする。
        if (other.gameObject.tag == "UFOBody")
        {
            this.transform.position = new Vector3(-40, 0, -40);
            this.uping = false;
            this.gameObject.SetActive(false);
            MobGenerator.generateCount--;
        }

        //床にフレた時、ジャンプをする動作と、吸い込みによって回転した値rotatereturnを使って元の向きに戻す処理をする。
        if (other.gameObject.tag == "Floor")
        {
            this.jumping = true;
            this.transform.Rotate(0, -this.rotatereturn, 0);
            this.rotatereturn = 0;
            this.dropDown = false;
        }

    }

    //DeadLineに触れたらモブキャラを非表示、ゲーム終了した時は再配置されないように処理
    void OnTriggerEnter(Collider triggerEnter)
    {
        if (triggerEnter.gameObject.tag == "DeadLine")
        {
            if (LifeController.isEnd == false)
            {
                MobGenerator.generateCount--;
            }
           
            this.gameObject.SetActive(false);
        }
    
    }

}

