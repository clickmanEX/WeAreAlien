using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

    public float scrollSpeed;
    private float deadLine = -100f;
    private float startLine = 200f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (LifeController.isEnd)
        {
            this.scrollSpeed *= 0.99f;
        }

        transform.Translate(0, this.scrollSpeed, 0);
        if (transform.position.z < this.deadLine)
        {
            transform.position = new Vector3(0, 0, this.startLine);
        }

    }
}
