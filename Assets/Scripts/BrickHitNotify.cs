using UnityEngine;

public class BrickHitNotify : MonoBehaviour
{
    // event
    public delegate void BrickHitEventHandler(GameObject brick, GameObject collider);
    public static event BrickHitEventHandler OnBrickHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnBrickHit?.Invoke(gameObject, collision.gameObject);
    }
}
