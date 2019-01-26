using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    public float Speed;
    public float JumpSpeed;
    public Collider2D Ground;

    private Rigidbody2D body;
    private bool grounded = false;

    // Start is called before the first frame update
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        var h = Input.GetAxisRaw("Horizontal") * Speed;
        var v = body.velocity.y;

        body.velocity = new Vector2(h, v);

        var jump = Input.GetAxisRaw("Vertical") * JumpSpeed;

        if (grounded && jump > 0) {
            body.velocity = new Vector2(body.velocity.x, jump);
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider == Ground) {
            foreach (var contact in other.contacts) {
                if (contact.normal == Vector2.up) {
                    grounded = true;
                    return;
                }
            }
        }
    }
}