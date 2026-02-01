using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RigidBodySleep (from Introduction to Game Design, Prototyping, and Development - Gibson Bond)
// prevents physics interactions for a few frames after instantiation

[RequireComponent( typeof(Rigidbody) )]
public class RigidBodySleep : MonoBehaviour
{
    private int sleepCountdown = 4;

    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if ( sleepCountdown > 0 )
        {
            rigid.Sleep();
            sleepCountdown--;
        }
    }
}
