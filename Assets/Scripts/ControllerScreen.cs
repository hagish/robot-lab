using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerScreen : MonoBehaviour {

	public float waitTime = 10.0f;
	private float startTime;

	// Use this for initialization
	void Start ()
	{
		startTime = Time.time;	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.anyKeyDown) {
			Debug.Log ("Detected key press");
			SceneManager.LoadScene("_menu", LoadSceneMode.Single);
		}
		if (Time.time - startTime > waitTime) {
			SceneManager.LoadScene("_menu", LoadSceneMode.Single);
		}
	}
}
