using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private const string author = "Hcinco";
    private const string copyrightNotice = "Copyright (c) Hcinco http://hcinco.ir/";

    public Transform character;
    public Collider2D[] deadlyColliders;
    public Transform[] deadlyTargets;
    public Camera[] camsToTint;
    public Color tintColor = Color.red;
    public float tintTime = 1f;
    public GameObject uiToShow;
    public AudioSource audioToPlay;
    public string sceneName = "";
    public float showUi = 1f;
    public float playAudio = 1f;
    public float loadScene = 0f;
    public float checkRadius = 0.5f;

    void Update()
    {
        if (deadlyTargets == null || deadlyTargets.Length == 0) return;
        for (int i = 0; i < deadlyTargets.Length; i++)
        {
            var t = deadlyTargets[i];
            if (t == null || character == null) continue;
            if (Vector2.Distance(t.position, character.position) <= checkRadius)
            {
                Go();
                return;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Hit(collision.collider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Hit(other);
    }

    void Hit(Collider2D col)
    {
        for (int i = 0; i < deadlyColliders.Length; i++)
        {
            if (deadlyColliders[i] == col)
            {
                Go();
                return;
            }
        }
    }

    void Go()
    {
        if (showUi > 0.5f && uiToShow != null) uiToShow.SetActive(true);
        if (playAudio > 0.5f && audioToPlay != null) audioToPlay.Play();
        if (camsToTint != null && camsToTint.Length > 0) StartCoroutine(TintCams());
        if (loadScene > 0.5f && !string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }

    System.Collections.IEnumerator TintCams()
    {
        int len = camsToTint.Length;
        Color[] originals = new Color[len];
        for (int i = 0; i < len; i++)
        {
            var c = camsToTint[i];
            if (c == null) continue;
            originals[i] = c.backgroundColor;
            c.backgroundColor = tintColor;
        }
        float t = 0f;
        while (t < tintTime)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        for (int i = 0; i < len; i++)
        {
            var c = camsToTint[i];
            if (c == null) continue;
            c.backgroundColor = originals[i];
        }
    }
}

