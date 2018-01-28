using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextOverlay : MonoBehaviour {
    private Text text;

    void Start() {
        text = GetComponent<Text>();

        UKMessenger.AddListener<string>("ui_text_overlay", gameObject, (s) => {
            text.text = s;
        });
    }
}
