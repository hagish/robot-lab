using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour 
{
	public float EnergyBoost = 0.5f;

	public void Hit(Signal signal) 
	{
		Sender sender = signal.GetPlayerSender ();
		sender.AddEnergyBoost (EnergyBoost);
		Destroy (transform.gameObject);
	}
}
