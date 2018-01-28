using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    
	public static LevelManager Instance;

	private AudioSource audioSource;
	public AudioClip bgMusicClip;
	public AudioClip crowdCheerClip;
	public AudioClip endLevelClip;

    private void Awake() {
        Instance = this;
		audioSource = GetComponent<AudioSource> ();
    }

    public int ScoreLeft;

    public bool CanSpawnMore() {
        return ScoreLeft > 0;
    }

    IEnumerator Start () {
        // stupid hack to wait for ui
        yield return new WaitForSeconds(1);

        UKMessenger.Broadcast<int>("score_left_set", ScoreLeft);

        UKMessenger.AddListener<int, int>("score_inc", gameObject, (playerId, inc) => {
            ScoreLeft -= inc;

			if (crowdCheerClip != null) {
    			audioSource.clip = crowdCheerClip;
				audioSource.Play();
			}

            if (ScoreLeft <= 0) {
                StartCoroutine(CoEndGame());
            }
       });

	}

    IEnumerator CoEndGame() {

		if (endLevelClip != null) {
			audioSource.clip = endLevelClip;
			audioSource.Play();
		}

        int score1 = 0;
        int score2 = 0;
        foreach (var exit in GameObject.FindObjectsOfType<AgentExit>()) {
            if (exit.playerExitId == 1) score1 += exit.scored;
            if (exit.playerExitId == 2) score2 += exit.scored;
        }

        string s = "";
        if (score1 == score2) s = "DRAW";
        else if (score1 > score2) s = "RED WINS";
        else if (score2 > score1) s = "BLUE WINS";
        else s = ":)";

        s += "\n\nESC TO CONTINUE";

        UKMessenger.Broadcast<string>("ui_text_overlay", s);

        yield return null;
    }
}
