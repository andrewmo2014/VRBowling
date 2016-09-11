using UnityEngine;
using System.Collections;

public class BallFollow : MonoBehaviour {

    public GameObject ball;
    private float fixedYPos;

    void Awake()
    {
        fixedYPos = transform.position.y;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(ball.transform.position.x, fixedYPos, ball.transform.position.z);
        transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
	}
}
