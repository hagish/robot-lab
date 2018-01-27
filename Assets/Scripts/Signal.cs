using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signal : MonoBehaviour
{
    SigPartSystem.SignalInfo info;

    public void SetCommand(SigPartSystem.SignalInfo info){
        this.info = info;
    }

	public string GetCommand() {return info.Command;}
	public int GetPlayerOriginId() {return info.PlayerOriginId;}
    public int GetSignalGroupId() {return info.SignalGroupId;}
    public Vector3 GetCommandDirection() {return info.CommandDirection;}
}
