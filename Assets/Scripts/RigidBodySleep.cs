using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RigidBodySleep (based on script of same name from
// "Introduction to Game Design, Prototyping, and Development" - Gibson Bond)
// prevents physics interactions for a few frames after instantiation

// Changes: WaitAndSleep coroutine instead of FixedUpdate() version
// also, disables script after sleeping is done (avoiding unnecessary updates)

[RequireComponent( typeof(Rigidbody) )]
public class RigidBodySleep : MonoBehaviour
{
    //private int sleepCountdown = 4;
    //int FixedUpdateCalls = 0;

    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        // start coroutine to wait a frame before sleeping
        StartCoroutine( WaitAndSleep() );
    }

    /*void FixedUpdate()
    {
        if ( sleepCountdown > 0 )
        {
            rigid.Sleep();
            sleepCountdown--;
        }
        Debug.Log("FixedUpdate call #: " + FixedUpdateCalls);
        FixedUpdateCalls++;
    }*/

    IEnumerator WaitAndSleep()
    {
        int sleeps = 4;
        int framesWaited = 0;
        while ( framesWaited < sleeps )
        {
            //Debug.Log("Frames waited: " + framesWaited);            
            // wait until FixedUpdate has been called
            yield return new WaitForFixedUpdate();
            rigid.Sleep();
            framesWaited++;
        }
        // disable this script
        this.enabled = false;
    }
}
