using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerState : MonoBehaviour {
    public string SnapshotName = "Game";

    IEnumerator Start () {
        while (MySceneManager.Instance == null) yield return null;
        MySceneManager.Instance.TransitionMixer(SnapshotName);
	}
}
