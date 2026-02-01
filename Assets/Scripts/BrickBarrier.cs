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
        totalBricksHit++;
        if (basicBricks.Contains(brick))
        {
            //basicBricks.Remove(brick);
            //Destroy(brick);
            Debug.Log("Basic brick hit: " + brick.name);
        }
        else if (headStoneBricks.Contains(brick))
        {
            // maybe play a sound or effect for headstone bricks
            Debug.Log("HeadStone brick hit: " + brick.name);
        }

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
            brickRb.AddForce(Vector3.forward * (totalBricksHit * 1.5f), ForceMode.Impulse);
        }
    }


}