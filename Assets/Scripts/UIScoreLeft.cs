using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreLeft : MonoBehaviour {
    
    private int score;
    private Text text;

    void Start() {
        text = GetComponent<Text>();

        UKMessenger.AddListener<int, int>("score_inc", gameObject, (playerId, inc) => {
            score -= inc;
            Refresh();
        });
        UKMessenger.AddListener<int>("score_left_set", gameObject, (val) => {
            score = val;
            Refresh();
        });

        Refresh();
    }

    private void Refresh() {
        text.text = string.Format("{0}", score);
    }
}
