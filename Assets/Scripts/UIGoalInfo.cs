using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGoalInfo : MonoBehaviour {

    public AnimationCurve Curve;
    public float Delay;
    public float FadeTime;

	IEnumerator Start () {
        yield return new WaitForSeconds(Delay);

        var text = GetComponent<Text>();

        var c = text.color;

        var t0 = Time.time;
        while (Time.time < t0 + FadeTime) {
            yield return null;
            var f = (Time.time - t0) / FadeTime;
            c.a = Curve.Evaluate(f);
            text.color = c;
        }
        gameObject.SetActive(false);
    }
}
