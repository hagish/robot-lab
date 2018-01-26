using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
	private string command;
	private int playerOriginId;
	private int signalGroupId;

	public void SetCommand(string _command, int _playerOriginId, int _signalGroupId)
	{
		command        = _command;
		playerOriginId = _playerOriginId;
		signalGroupId  = _signalGroupId;
	}

	public string GetCommand() {return command;}
	public int GetPlayerOriginId() {return playerOriginId;}
	public int GetSignalGroupId() {return signalGroupId;}
}
