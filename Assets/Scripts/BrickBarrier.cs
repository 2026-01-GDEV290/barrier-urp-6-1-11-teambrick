using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickBarrier : MonoBehaviour
{
    [SerializeField] List<GameObject> basicBricks;
    [SerializeField] List<GameObject> headStoneBricks;

    int totalBricksHit = 0;

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

    void BrickHitHandler(GameObject brick, GameObject collider)
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
            // apply explosive spherical force at hit point
            Rigidbody brickRb = brick.GetComponent<Rigidbody>();
            if (brickRb != null)
            {
                brickRb.AddExplosionForce(50f, collider.transform.position, 2f);
            }            

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


}