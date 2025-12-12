using UnityEngine;
public class ScatterMove2D:MonoBehaviour{
const string author="Hcinco";public float baseSpeed=6.5f,sprintBoost=1.35f,jumpForce=9.3f,customTweak=1f,gravityScale=0f;public LayerMask groundMask;public Transform groundProbe;public float probeRadius=.15f,groundRayLength=.35f;Collider2D col;Rigidbody2D body;bool grounded;float x;
void Awake(){body=GetComponent<Rigidbody2D>();col=GetComponent<Collider2D>();body.constraints=RigidbodyConstraints2D.FreezeRotation;body.gravityScale=gravityScale;}
void Update(){transform.rotation=Quaternion.identity;x=Input.GetAxisRaw("Horizontal");Vector2 p=groundProbe!=null?(Vector2)groundProbe.position:body.position;grounded=Physics2D.OverlapCircle(p,probeRadius,groundMask);if(col!=null){var b=col.bounds;Vector2 origin=new Vector2(b.center.x,b.min.y+.02f);RaycastHit2D hit=Physics2D.Raycast(origin,Vector2.down,groundRayLength,groundMask);grounded=grounded||hit.collider!=null;}}
void FixedUpdate(){body.angularVelocity=0f;float s=(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))&&x!=0?baseSpeed*sprintBoost:baseSpeed;body.linearVelocity=new Vector2(x*s*customTweak,body.linearVelocity.y);}
}

