using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public float speed = 0.06f;
    public float damage = 15f;

    void FixedUpdate()
    {
        // Move up
        transform.position = new Vector2(
        transform.position.x, 
        transform.position.y + speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Boundary" && col.gameObject.name == "BulletDeleteBoundary") 
        {
            Explode();
        }

        if (col.gameObject.tag == "Astroid") 
        {
            AstroidManager astroid = col.collider.GetComponent<AstroidManager>();
            if (astroid != null) {
                astroid.takeDamage(damage);
                Explode();
            }
        }
    }

    void Explode()
    {
        Destroy(this.gameObject);
    }
}
