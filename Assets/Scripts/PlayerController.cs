using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Transform head;
    public Camera playerCamera;

    private Transform weapon;

    bool attacking = false;
    bool retracting = false;

    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -30f;

    public float swingSpeed = 250f;

    private float rotationY;
    private float verticalVelocity;

    public Vector3 newVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rotationY = transform.localEulerAngles.y;
    }

    void Start()
    {
        weapon = GameObject.Find("SledgePrimitive").transform;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        //rb.linearVelocity = transform.TransformDirection(newVelocity);
        if (attacking)
        {
            if (weapon.rotation.eulerAngles.x >= 90f)
            {
                attacking = false;
                retracting = true;
                weapon.rotation = Quaternion.Euler(90f, weapon.rotation.eulerAngles.y, weapon.rotation.eulerAngles.z);
            }
            else
                weapon.Rotate(Vector3.right * swingSpeed * Time.fixedDeltaTime);
        }
        else if (retracting)
        {
            //Debug.Log("Retracting, angle: " + weapon.rotation.eulerAngles.x);
            if (weapon.rotation.eulerAngles.x <= 0f || weapon.rotation.eulerAngles.x >= 180f)
            {
                retracting = false;
                weapon.rotation = Quaternion.Euler(0f, weapon.rotation.eulerAngles.y, weapon.rotation.eulerAngles.z);
            }
            else
                weapon.Rotate(Vector3.left * swingSpeed * Time.fixedDeltaTime);
        }
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

    public void Attack()
    {
        if (!attacking && !retracting)
        {
            attacking = true;
        }
        //Debug.Log("Attack triggered");
    }

    public void MeleeHit(GameObject hitObject)
    {
        Debug.Log("PlayerController registered melee hit on: " + hitObject.name);
        if (!retracting)
        {
            if (attacking)
            {
                attacking = false;
                retracting = true;
            }
        }
    }

}
