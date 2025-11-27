using UnityEngine;
using TMPro;
using System.Collections;

public class MessageBanner : MonoBehaviour
{
    public TMP_Text label;
    public float fadeTime = 0.25f;

    Coroutine running;

    public void Show(string msg, float hold = 0.8f)
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(CoShow(msg, hold));
    }

    IEnumerator CoShow(string msg, float hold)
    {
        if (!label) yield break;
        label.gameObject.SetActive(true);
        label.text = msg;
        var c = label.color; c.a = 0f; label.color = c;

        // fade in
        float t = 0f;
        while (t < fadeTime) { t += Time.unscaledDeltaTime; c.a = Mathf.Lerp(0f, 1f, t/fadeTime); label.color = c; yield return null; }

        // hold
        yield return new WaitForSecondsRealtime(hold);

        // fade out
        t = 0f;
        while (t < fadeTime) { t += Time.unscaledDeltaTime; c.a = Mathf.Lerp(1f, 0f, t/fadeTime); label.color = c; yield return null; }

        label.gameObject.SetActive(false);
        running = null;
    }
}
