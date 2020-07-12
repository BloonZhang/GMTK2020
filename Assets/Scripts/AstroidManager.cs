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
    private bool hitPlayer = false; // can't hit player twice

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
        // If hits the despawn zone
        if (col.gameObject.tag == "Boundary" && col.gameObject.name == "AstroidDeleteBoundary") 
        {
            Explode();
        }

        // If hits player
        if (col.gameObject.tag == "Player" && hitPlayer == false) 
        {
            ShipManager ship = col.collider.GetComponent<ShipManager>();
            if (ship != null) {
                ship.hitAstroid();
                hitPlayer = true;   // can't get hit again
                StartCoroutine(SlowDown());
            }        
        }
    }

    // public methods
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log(currentHealth);
        if (currentHealth <= 0) {Explode();}
    }

    // helper methods
    private IEnumerator SlowDown()
    {
        float originalSpeed = speed;
        float speedScale = 1f;
        while(speed > originalSpeed * 0.5)
        {
            speed = originalSpeed * speedScale;
            speedScale -= 0.1f;
            yield return new WaitForSeconds(0.1f); // 1 seconds to slow down
        }

    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
