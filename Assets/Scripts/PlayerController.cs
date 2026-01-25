using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera playerCamera;

    [Header("Configuration")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public float gravityScale = 1f;

    [Header("Runtime")]
    public Vector3 newVelocity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        Vector3 headRotation = head.localEulerAngles;
        headRotation.x = RestrictAngle(headRotation.x - Input.GetAxis("Mouse Y") * mouseSensitivity, -90f, 90f);
        head.localEulerAngles = headRotation;

        newVelocity = Vector3.up * rb.linearVelocity.y;
        float speed = moveSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    void LateUpdate()
    {
      Vector3 e = head.eulerAngles;
      e.x -= Input.GetAxis("Mouse Y") * mouseSensitivity;
      e.x = RestrictAngle(e.x, -85f, 85f);  
      head.eulerAngles = e;
    }

    public float RestrictAngle(float angle, float angleMin, float angleMax)
    {
        if (angle > 180f) angle -= 360f;
        else if (angle < -180f) angle += 360f;
        return Mathf.Clamp(angle, angleMin, angleMax);
    }

    void OnCollisionStay(Collision collision)
    {
        
    }

    void OnCollisionExit(Collision collision)
    {
        
    }

}
