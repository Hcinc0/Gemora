using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverTrigger : MonoBehaviour
{
    private const string author = "Hcinco";
    private const string copyrightNotice = "Copyright (c) Hcinco http://hcinco.ir/";

    public Transform character;
    public Collider2D[] deadlyColliders;
    public Transform[] deadlyTargets;
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
        if (loadScene > 0.5f && !string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }
}

