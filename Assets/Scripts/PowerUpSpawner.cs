using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
	public GameObject[] powerUpPrefabs;
	public float maxSpawnTimeInterval;
	public float minSpawnTimeInterval;

	private float timeLastSpawn;
	private float timeNextSpawn;

	// Use this for initialization
	void Start ()
	{
		timeLastSpawn = Time.time;
		timeNextSpawn = timeLastSpawn + Random.Range (minSpawnTimeInterval, maxSpawnTimeInterval);
		Debug.Log ("No. of power-ups : " + powerUpPrefabs.Length + "   tNext : " + timeNextSpawn);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time >= timeNextSpawn) {
			int numPowerUps = powerUpPrefabs.Length;
			if (numPowerUps > 0) {
				int iPowerUp = Random.Range (0, numPowerUps - 1);
				Instantiate (powerUpPrefabs [iPowerUp], transform.position, Quaternion.identity);
				Debug.Log ("Instantiated power-up : " + iPowerUp + "   tNext : " + timeNextSpawn);
			}
			timeLastSpawn = Time.time;
			timeNextSpawn = timeLastSpawn + Random.Range (minSpawnTimeInterval, maxSpawnTimeInterval);
		}
	}
}
