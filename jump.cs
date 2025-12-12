using UnityEngine;public class jump:MonoBehaviour{public Rigidbody2D rb;public Transform character;public KeyCode key=KeyCode.Space;public float force=9f;void Update(){if(rb==null)return;if(Input.GetKeyDown(key)){Vector2 v=rb.linearVelocity;v.y=force;rb.linearVelocity=v;}}}

