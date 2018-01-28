using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnInnerPlayers : MonoBehaviour {
    private AvatarController[] players;
    private Bounds bounds;

    void OnEnable () {
        players = GameObject.FindObjectsOfType<AvatarController>();
        bounds = GetComponent<BoxCollider>().bounds;
	}
	
	void Update () {
        if (players != null) {
            foreach (var player in players) {
                if (bounds.Contains(player.transform.position)) {
                    player.GetComponent<Respawn>().DoRespawn();
                }
            }
        }
	}
}
