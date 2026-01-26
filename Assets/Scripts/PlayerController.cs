using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Transform head;
    public Camera playerCamera;

    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -30f;

    private float rotationY;
    private float verticalVelocity;

    public Vector3 newVelocity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //rb.linearVelocity = transform.TransformDirection(newVelocity);
    }

    void LateUpdate()
    {   
    }

    public void Move(Vector2 moveVector)
    {
        Vector3 move = transform.forward * moveVector.y + transform.right * moveVector.x;
        move = move * moveSpeed * Time.deltaTime;
        characterController.Move(move);

        verticalVelocity += gravity * Time.deltaTime;
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    public void Rotate(Vector2 lookVector)
    {
        rotationY += lookVector.x * rotateSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

}
