using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour {
    public int PlayerId;

    private int score;
    private Text text;

	void Start () {
        text = GetComponent<Text>();

        UKMessenger.AddListener<int, int>("score_inc", gameObject, (playerId, inc) => {
            if (playerId == PlayerId) {
                score += inc;
                Refresh();
            }
        });
        UKMessenger.AddListener<int, int>("score_set", gameObject, (playerId, val) => {
            if (playerId == PlayerId) {
                score = val;
                Refresh();
            }
        });

        Refresh();
	}

    private void Refresh() {
        text.text = string.Format("{0}", score);
    }
}
