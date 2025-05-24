using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile object
/// </summary>
public class Projectile : MonoBehaviour {
    public float baseSpeed = 3;
    public float damage;

    public bool destroyOnHit;
    public GameObject onHitEffect;

    Rigidbody2D rb;

    AudioSource music; //sound of hit

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        //if the velocity is too low increase it
        if (Mathf.Abs(rb.velocity.y) < 1.5f && Mathf.Abs(rb.velocity.x) < 1.5f && !destroyOnHit) {
            rb.velocity = Vector2.down * baseSpeed;
        }
    }

    /// <summary>
    /// Trigger collision event
    /// If the Projectile hits the bottom boundary, it deactivates itself and goes back to Projectile pool
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Boundary") {
            ResetProjectile();
        }
    }

    /// <summary>
    /// Collision event
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision) {
        if (rb.velocity.y < 1 && rb.velocity.y > -1) {
            rb.velocity += Vector2.down * 2f;
        }

        if (collision.gameObject.tag.Equals("Trash") && destroyOnHit) {
            if (onHitEffect != null) {
                var g = Instantiate(onHitEffect);
                g.transform.position = transform.position;
            }           

            ResetProjectile();
        }

        //if (SoundManager._instance.hasSound)
        //music.Play();
    }

    /// <summary>
    /// Resetting Projectile to default options
    /// </summary>
    void ResetProjectile() {
        gameObject.SetActive(false);
        GetComponent<Rigidbody2D>().gravityScale = 0;

        foreach (TrailRenderer t in GetComponentsInChildren<TrailRenderer>()) {
            t.Clear();
        }
    }
}
