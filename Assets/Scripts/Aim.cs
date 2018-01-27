using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{    
	public GameObject Root;
	public GameObject ConeLeft;
	public GameObject ConeRight;

    public float lerpSpeed = 0.005f;
	public float MinAngleInDegrees = 10.0f;
	public float MaxAngleInDegrees = 180.0f;
	public float InitConeFraction = 0.5f;
	public float OpeningSpeed = 1.0f;

	private float AngleInDegrees;
	private float ConeFraction;

	private Vector3 direction;
    private Vector3 lastDirection = Vector3.zero;


	void Awake()
	{
		ConeFraction = InitConeFraction;
		AngleInDegrees = Mathf.Lerp (MinAngleInDegrees, MaxAngleInDegrees, ConeFraction);
		Debug.Log ("Set cone angle : " + ConeFraction + "  " + AngleInDegrees);
	}

	void Update()
	{
		AngleInDegrees = Mathf.Lerp (MinAngleInDegrees, MaxAngleInDegrees, ConeFraction);
		Quaternion lastAngle = Quaternion.LookRotation(lastDirection);
		Quaternion newAngle = Quaternion.LookRotation(direction);
		lastDirection = direction;
		Root.transform.rotation = Quaternion.RotateTowards(lastAngle, newAngle, lerpSpeed);
		float directionAngle = Root.transform.eulerAngles.y;
		Debug.Log ("DIRECTION ANGLE : " + directionAngle);
		ConeLeft.transform.eulerAngles = new Vector3 (0.0f, directionAngle - 0.5f * AngleInDegrees, 0.0f);
		ConeRight.transform.eulerAngles = new Vector3 (0.0f, directionAngle + 0.5f * AngleInDegrees, 0.0f);
	}

	public void IncreaseConeAngle()
	{
		ConeFraction += (Time.deltaTime * OpeningSpeed);
		ConeFraction = Mathf.Min (1.0f, ConeFraction);
		AngleInDegrees = Mathf.Lerp (MinAngleInDegrees, MaxAngleInDegrees, ConeFraction);
		Debug.Log ("Increased cone angle : " + ConeFraction + "  " + AngleInDegrees + "  " + MinAngleInDegrees + "  " + MaxAngleInDegrees);
	}

	public void DecreaseConeAngle()
	{
		ConeFraction -= (Time.deltaTime * OpeningSpeed);
		ConeFraction = Mathf.Max (0.0f, ConeFraction);
		AngleInDegrees = Mathf.Lerp (MinAngleInDegrees, MaxAngleInDegrees, ConeFraction);
		Debug.Log ("Decreased cone angle : " + ConeFraction + "  " + AngleInDegrees + "  " + MinAngleInDegrees + "  " + MaxAngleInDegrees);
	}

	public void SetDirection (Vector3 newDirection)
	{
		if (newDirection == Vector3.zero) {
			direction = lastDirection;
		} else {
			direction = newDirection;
		}
    }

	public float GetAngleInDegrees()
	{
		return AngleInDegrees;
	}

	public float GetConeFraction()
	{
		return ConeFraction;
	}
}
