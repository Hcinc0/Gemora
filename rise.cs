using UnityEngine;

public class rise : MonoBehaviour
{
    private const string author = "Hcinco";

    public float speed = 0.5f;
    public float pick;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}

