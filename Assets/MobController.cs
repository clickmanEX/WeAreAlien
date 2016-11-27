using UnityEngine;
using System.Collections;

public class MobController : MonoBehaviour
{

    private Rigidbody mobRigidbody;
    private GameObject ufo;
    private float movetime = 1.5f;
    public float forwardForce = -10f;   //基本血：-10f　車などの早く動かしたいものはインスペクタで別途調整
    public float upForce = 300f;        //基本値：300f　車などの跳ねさせたくないものはインスペクタで0に調整
    private float upDistance;
    private float upSpeed;
    private float rotatespeed = 2f;
    private bool uping = false;
    private bool jumping = false;
    public static bool mobActive = true;


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
        this.transform.Translate(0, 0, Time.deltaTime * forwardForce);

        if (this.jumping)
        {
            this.mobRigidbody.AddForce(this.transform.up * this.upForce);
            this.jumping = false;
        }

        if (this.uping)
        {
            this.transform.Rotate(0, rotatespeed, 0);
            this.mobRigidbody.isKinematic = true;
            this.transform.Translate(0, Time.deltaTime * upSpeed, 0);
        }else
        {
            this.mobRigidbody.isKinematic = false;
        }

        if(mobActive == false)
        {
            this.gameObject.SetActive(false);
        }

    }

    void OnTriggerStay(Collider triggerStay)
    {
        this.uping = false;

        if (Input.GetKey(KeyCode.Space) || UFOController.isCatchButtonDown)
        {
            this.uping = true;
            this.transform.position = new Vector3(ufo.transform.position.x, this.transform.position.y, ufo.transform.position.z);

        }

    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "UFOBody")
        {
            this.transform.position = new Vector3(-40, 0, -40);
            this.uping = false;
            this.gameObject.SetActive(false);
            MobGenerator.generate = true;
            

        }

        if (other.gameObject.tag == "Floor")
        {
            this.jumping = true;
        }

    }

    void OnTriggerEnter(Collider triggerEnter)
    {
        if(triggerEnter.gameObject.tag == "DeadLine")
        {
            this.gameObject.SetActive(false);
            MobGenerator.generate = true;
            
        }
        
    }


}

