using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    PlayerController playerController;
    public GameObject decalPrefab; // Assign a prefab with DecalProjector component
    void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Melee weapon hit: " + other.gameObject.name);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Melee weapon collided with: " + collision.gameObject.name);
        playerController.MeleeHit(collision.gameObject);

        // Spawn decal at hit point
        ContactPoint contact = collision.contacts[0];
        Vector3 decalPosition = contact.point - contact.normal * 0.25f; // Offset INTO the surface
        GameObject decal = Instantiate(decalPrefab, decalPosition, Quaternion.LookRotation(contact.normal));
        decal.transform.SetParent(collision.transform);
    }
}
