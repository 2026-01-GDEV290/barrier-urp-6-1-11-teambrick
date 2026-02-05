using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickBarrier : MonoBehaviour
{
    [SerializeField] List<GameObject> basicBricks;
    [SerializeField] List<GameObject> headStoneBricks;
    [SerializeField] AudioClip brickHitOtherSound;

    int totalBricksHit = 0;

    // destroyed delegate event
    public delegate void BrickBarrierDestroyed();
    public static event BrickBarrierDestroyed OnBrickBarrierDestroyed;

    void Awake()
    {
        // add every game object with Brick tag
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (GameObject brick in bricks)
        {
            if (brick.name.Contains("Headstone"))
            {
                headStoneBricks.Add(brick);
            }
            else
            {
                basicBricks.Add(brick);
            }
        }
    }

    void OnEnable()
    {
        BrickHitNotify.OnBrickHit += BrickHitHandler;
    }

    void OnDisable()
    {
        BrickHitNotify.OnBrickHit -= BrickHitHandler;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BrickHitHandler(GameObject brick, GameObject hitObject, Vector3 hitPoint)
    {
        Debug.Log("Brick " + brick.name + " registered brick hit notify on: " + hitObject.name);

        // play other brick hit sound
        if (brickHitOtherSound != null)
        {
            AudioSource.PlayClipAtPoint(brickHitOtherSound, hitPoint);
        }
    }

    void BrickHitHandleOld(GameObject brick, GameObject collider)
    {
        // ignore hits from other bricks
        //if (basicBricks.Contains(collider) || headStoneBricks.Contains(collider))

        // Only register hits from sledgehammer
        if (!collider.name.Contains("Sledge"))
        {
            return;
        }

        totalBricksHit++;
        
        if (basicBricks.Contains(brick))
        {
            //basicBricks.Remove(brick);
            //Destroy(brick);
            Debug.Log("Basic brick hit: " + brick.name + " hit source: " + collider.name);
        }
        else if (headStoneBricks.Contains(brick))
        {
            // maybe play a sound or effect for headstone bricks
            Debug.Log("HeadStone brick hit: " + brick.name + " hit source: " + collider.name);
        }

        if (totalBricksHit > 3)
        {
            // add gravity to all bricks 1st
            foreach (GameObject b in basicBricks)
            {
                Rigidbody rb = b.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                }
            }
            foreach (GameObject b in headStoneBricks)
            {
                Rigidbody rb = b.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                }
            }
            // Apply explosive spherical force at hit point
            Rigidbody brickRb = brick.GetComponent<Rigidbody>();
            if (brickRb != null)
            {
                Vector3 explosionPosition = collider.transform.position;
                explosionPosition.y = 0.5f;
                float explosionRadius = 2f;  // Adjust this to affect more/fewer bricks
                float explosionForce = 250f;

                // Find all rigidbodies in the explosion radius
                Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
                foreach (Collider col in colliders)
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                    }
                }
            }

            OnBrickBarrierDestroyed?.Invoke();

        }
        else
        {
            // apply movment force to brick based on collider's velocity
            //Rigidbody brickRb = brick.GetComponent<Rigidbody>();
            //Rigidbody colliderRb = collider.GetComponent<Rigidbody>();
            //if (brickRb != null && colliderRb != null)
            //{
            //    brickRb.AddForce(colliderRb.linearVelocity, ForceMode.Impulse);
            //}

            // apply Z axis positive movement force to brick
            Rigidbody brickRb = brick.GetComponent<Rigidbody>();
            if (brickRb != null)
            {
                brickRb.AddForce(Vector3.forward * Math.Clamp(totalBricksHit * 1.5f, 1.5f, 10f), ForceMode.Impulse);
            }
        }
    }

    public void BrickBashRespond(GameObject brick, Vector3 hitPoint)
    {
        Rigidbody brickRb = brick.GetComponent<Rigidbody>();
        if (brickRb != null)
        {
            brickRb.useGravity = true;
            brickRb.AddForce(Vector3.forward * 5f, ForceMode.Impulse);
        }
    }

    public void BrickExplode(GameObject brick, Vector3 explosionPoint)
    {

        foreach (GameObject b in basicBricks)
        {
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
        foreach (GameObject b in headStoneBricks)
        {
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
        // Apply explosive spherical force at hit point
        Rigidbody brickRb = brick.GetComponent<Rigidbody>();
        if (brickRb != null)
        {
            Vector3 explosionPosition = explosionPoint;
            explosionPosition.y = 0.5f;
            explosionPosition.z -= 0.5f;    // slightly behind hit point
            float explosionRadius = 2f;  // Adjust this to affect more/fewer bricks
            float explosionForce = 300f;

            // Find all rigidbodies in the explosion radius
            Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
                }
            }
        }

        OnBrickBarrierDestroyed?.Invoke();
    }


}