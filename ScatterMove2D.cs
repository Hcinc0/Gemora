using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScatterMove2D : MonoBehaviour
{
    private const string author = "Hcinco";
    private const string copyrightNotice = "Copyright (c) Hcinco http://hcinco.ir/";

    public float baseSpeed = 6.5f;
    public float sprintBoost = 1.35f;
    public float jumpForce = 9.3f;
    public float customTweak = 1f;
    public float gravityScale = 0f;
    public LayerMask groundMask;
    public Transform groundProbe;
    public float probeRadius = 0.15f;
    public float groundRayLength = 0.35f;

    public Collider2D[] deadlyColliders;
    public Transform[] deadlyTargets;
    public float checkRadius = 0.5f;
    public Camera[] camsToTint;
    public Color tintColor = Color.red;
    public float tintTime = 1f;
    public GameObject uiToShow;
    public AudioSource audioToPlay;
    public string sceneName = "";
    public float showUi = 1f;
    public float playAudio = 1f;
    public float loadScene = 0f;
    public float pauseOnDeath = 1f;

    private Collider2D col;
    private Rigidbody2D body;
    private bool grounded;
    private float x;
    private bool dead;
    private float baseScaleX;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        body.gravityScale = gravityScale;
        baseScaleX = transform.localScale.x == 0 ? 1f : transform.localScale.x;
    }

    void Update()
    {
        if (dead) return;
        transform.rotation = Quaternion.identity;
        x = Input.GetAxisRaw("Horizontal");
        float dir = x > 0.01f ? 1f : x < -0.01f ? -1f : 0f;
        if (dir != 0f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(baseScaleX) * dir;
            transform.localScale = s;
        }

        Vector2 p = groundProbe != null ? (Vector2)groundProbe.position : body.position;
        grounded = Physics2D.OverlapCircle(p, probeRadius, groundMask);
        if (col != null)
        {
            var b = col.bounds;
            Vector2 origin = new Vector2(b.center.x, b.min.y + 0.02f);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundRayLength, groundMask);
            grounded = grounded || hit.collider != null;
        }

        if (deadlyTargets != null && deadlyTargets.Length > 0 && col != null)
        {
            for (int i = 0; i < deadlyTargets.Length; i++)
            {
                var t = deadlyTargets[i];
                if (t == null) continue;
                if (Vector2.Distance(t.position, col.bounds.center) <= checkRadius)
                {
                    Go();
                    return;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (dead) return;
        body.angularVelocity = 0f;
        float s = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && x != 0 ? baseSpeed * sprintBoost : baseSpeed;
        body.linearVelocity = new Vector2(x * s * customTweak, body.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (dead) return;
        Hit(collision.collider);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;
        Hit(other);
    }

    void Hit(Collider2D colHit)
    {
        if (deadlyColliders == null) return;
        for (int i = 0; i < deadlyColliders.Length; i++)
        {
            if (deadlyColliders[i] == colHit)
            {
                Go();
                return;
            }
        }
    }

    void Go()
    {
        if (dead) return;
        dead = true;
        if (pauseOnDeath > 0.5f) Time.timeScale = 0f;
        if (showUi > 0.5f && uiToShow != null) uiToShow.SetActive(true);
        if (playAudio > 0.5f && audioToPlay != null) audioToPlay.Play();
        if (camsToTint != null && camsToTint.Length > 0) StartCoroutine(TintCams());
        if (loadScene > 0.5f && !string.IsNullOrEmpty(sceneName)) SceneManager.LoadScene(sceneName);
    }

    IEnumerator TintCams()
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

