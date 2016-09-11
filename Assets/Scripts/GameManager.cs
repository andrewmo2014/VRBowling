using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private Transform[] pinsArray;
    public float EPSILON = 0.25f;

    public Text scoreBoardResult;
    private int score = 0;
    private Color originalScoreColor = Color.red;

    public GameObject pinsPrefab;
    public GameObject ballPrefab;

    private GameObject pinsRef;
    private GameObject ballRef;

    public SteamVR_TrackedObject controller;  //Reference to the controller

    // Use this for initialization
    void Start () {

        pinsRef = GameObject.FindGameObjectWithTag("Pins");
        ballRef = GameObject.FindGameObjectWithTag("Ball");

        pinsArray = SetPinsArray();
	
	}
	
	// Update is called once per frame
	void Update () {

        //Get a reference to the steam controller we drag to the script:
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.index);

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            ResetScore();
            ResetPins();
            ResetBall();
        }

        CalculateScore();
	}

    /// <summary>
    /// Updates the Score
    /// </summary>
    private void UpdateScore()
    {
        Color col = originalScoreColor; 
        if( score >= 0 && score < 4){           col = Color.red;    }
        else if( score >= 4 && score <= 7){     col = Color.yellow; }
        else if( score >= 8 && score <= 10){    col = Color.green;  }
        else{                                   col = originalScoreColor;    }

        scoreBoardResult.color = col;
        scoreBoardResult.text = score.ToString();

    }

    /// <summary>
    /// Resets the Score
    /// </summary>
    private void ResetScore()
    {
        score = 0;
        scoreBoardResult.color = originalScoreColor;
        scoreBoardResult.text = score.ToString();
    }

    /// <summary>
    /// Resets the pins
    /// </summary>
    public void ResetPins()
    {
        Destroy(pinsRef);
        GameObject newPins = GameObject.Instantiate(pinsPrefab) as GameObject;
        pinsRef = newPins;

        pinsArray = SetPinsArray();
    }

    /// <summary>
    /// Resets the ball
    /// </summary>
    private void ResetBall()
    {
        Destroy(ballRef);
        GameObject newBall = GameObject.Instantiate(ballPrefab) as GameObject;
        ballRef = newBall;
    }

    /// <summary>
    /// Calculates the current score;
    /// </summary>
    private void CalculateScore()
    {
        int counter = 0;
        foreach (Transform pin in pinsArray)
        {
            float dotProduct = Vector3.Dot(pin.forward, Vector3.up);
            if (Mathf.Abs(dotProduct) <= EPSILON) { counter += 1; }
        }
        if (score != counter)
        {
            score = counter;
            UpdateScore();
        }
    }

    /// <summary>
    /// Sets pin array
    /// </summary>
    /// <returns></returns>
    private Transform[] SetPinsArray()
    {

        int numberPins = pinsRef.transform.childCount;
        Transform[] pinsArrayTemp = new Transform[numberPins];

        for ( int i=0; i<numberPins; i++)
        {
            pinsArrayTemp[i] = pinsRef.transform.GetChild(i);
        }

        return pinsArrayTemp;
    }
}
