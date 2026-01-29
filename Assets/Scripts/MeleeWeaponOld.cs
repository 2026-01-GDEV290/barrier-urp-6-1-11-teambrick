using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HitDecalInfoO
{
    public float hitTime;
    public GameObject decal;
}

public class MeleeWeaponOld : MonoBehaviour
{
    PlayerControllerPrimitive playerController;
    public GameObject decalPrefab; // Assign a prefab with DecalProjector component

    List<HitDecalInfoO> hitDecals = new List<HitDecalInfoO>();

    public float hitRecoverTime = 0.50f;
    private float lastHitTime = 0f;

    public float decalFadeTime = 10f;

    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerControllerPrimitive>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float curTime = Time.time;
        if (hitDecals.Count > 0)
        {
            for (int i = hitDecals.Count - 1; i > -1; i--)
            {
                if (curTime > hitDecals[i].hitTime + decalFadeTime)
                {
                    GameObject.Destroy(hitDecals[i].decal);
                    hitDecals.RemoveAt(i);
                }
            }
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Melee weapon hit: " + other.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Melee weapon collided with: " + collision.gameObject.name);
        playerController.MeleeHit(collision.gameObject);

        Debug.Log($"Time.time {Time.time}, lastHitTime: {lastHitTime}, hitRecoverTime: {hitRecoverTime}");
        if (Time.time > lastHitTime + hitRecoverTime)
        {
            // Spawn decal at hit point
            ContactPoint contact = collision.contacts[0];
            Vector3 decalPosition = contact.point - contact.normal * 0.25f; // Offset INTO the surface
            GameObject decal = Instantiate(decalPrefab, decalPosition, Quaternion.LookRotation(contact.normal));
            decal.transform.SetParent(collision.transform);

            lastHitTime = Time.time;

            hitDecals.Add(new HitDecalInfoO { hitTime = lastHitTime, decal = decal} );           
        }
    }
}
