using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidManager : MonoBehaviour
{

    // public variables
    public float minSizeScale = 0.75f;
    public float maxSizeScale = 2.5f;

    // private variables
    private float sizeScale = 1.0f;
    private float baseSpeed = 0.03f;
    private float referenceForSpeed = 2.5f;
    private float speed = 1.0f;
    private float currentHealth = 50f;

    // Awake
    void Awake()
    {
        // Randomize the scale
        sizeScale = Random.Range(minSizeScale, maxSizeScale);
        gameObject.transform.localScale = new Vector3(sizeScale, sizeScale, 1.0f);
        speed = (1 / (sizeScale / referenceForSpeed)) * baseSpeed;
        currentHealth = sizeScale * currentHealth;
    }

    void FixedUpdate()
    {
        // Move up
        transform.position = new Vector2(
        transform.position.x, 
        transform.position.y - speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Boundary" && col.gameObject.name == "AstroidDeleteBoundary") 
        {
            Explode();
        }

        if (col.gameObject.tag == "Player") 
        {
            
        }
    }

    // public methods
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0) {Explode();}
    }

    // helper methods
    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
