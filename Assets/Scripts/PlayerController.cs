using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Transform head;
    public Camera playerCamera;

    private Transform weapon;
    private Collider weaponCollider;
    private BrickBarrier brickBarrier;

    bool attacking = false;
    bool retracting = false;

    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;
    public float jumpForce = 5f;
    public float gravity = -30f;

    public float swingSpeed = 250f;

    [SerializeField] private float cameraShakeDuration = 0.1f;
    [SerializeField] private float cameraShakeStrength = 0.15f;

    private Vector3 cameraOriginalPosition;
    private float rotationX;
    private float rotationY;
    private float verticalVelocity;

    private int hitCount = 0;

    public Vector3 newVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rotationY = transform.localEulerAngles.y;
        rotationX = transform.localEulerAngles.x;
        brickBarrier = FindFirstObjectByType<BrickBarrier>();
    }

    void OnEnable()
    {
        MeleeWeapon.OnHit += MeleeHit;
    }

    void OnDisable()
    {
        MeleeWeapon.OnHit -= MeleeHit;
    }

    void Start()
    {
        //weapon = GameObject.Find("SledgePrimitive").transform;
        weapon = GameObject.Find("SledgeHammer").transform;
        //set collider to isTrigger
        weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.isTrigger = true;

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
            //if (weapon.rotation.eulerAngles.x >= 90f)
            if (weapon.localRotation.eulerAngles.z >= 45f)
            {
                attacking = false;
                retracting = true;
                weapon.localRotation = Quaternion.Euler(weapon.localRotation.eulerAngles.x, weapon.localRotation.eulerAngles.y, 45f);
                weaponCollider.isTrigger = true;
            }
            else
                weapon.Rotate(Vector3.forward * swingSpeed * Time.fixedDeltaTime, Space.Self);
        }
        else if (retracting)
        {
            //Debug.Log("Retracting, angle: " + weapon.rotation.eulerAngles.x);
            //if (weapon.rotation.eulerAngles.x <= 0f || weapon.rotation.eulerAngles.x >= 180f)
            if (weapon.localRotation.eulerAngles.z <= 0f || weapon.localRotation.eulerAngles.z >= 180f)
            {
                retracting = false;
                weapon.localRotation = Quaternion.Euler(weapon.localRotation.eulerAngles.x, weapon.localRotation.eulerAngles.y, 0);
            }
            else
                weapon.Rotate(Vector3.back * swingSpeed * Time.fixedDeltaTime, Space.Self);
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
        // x-axis of mouse controls pitch (looking up/down)
        rotationY += lookVector.x * rotateSpeed * Time.deltaTime;
        rotationX -= lookVector.y * rotateSpeed * Time.deltaTime;
        //rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
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
            weaponCollider.isTrigger = false;
        }
        //Debug.Log("Attack triggered");
    }

    public void MeleeHit(GameObject hitObject, Vector3 hitPoint)
    {
        Debug.Log("PlayerController registered melee hit on: " + hitObject.name);
        
        if (!retracting)
        {
            if (attacking)
            {
                if (hitObject.CompareTag("Brick"))
                {
                    hitCount++;
                    if (hitCount > 3)
                        hitCount = 0;
                    
                    cameraShakeDuration = Mathf.Clamp(hitCount * 0.33f, 0.33f, 2f);
                    cameraShakeStrength = Mathf.Clamp(hitCount * 0.05f, 0.05f, 0.3f);
                    StartCoroutine(CameraShake());
                    Debug.Log("Hit brick");

                    if (hitCount == 3)
                    {
                        // Trigger brick barrier destruction or other effects
                        if (brickBarrier != null)
                        {
                            brickBarrier.BrickExplode(hitObject, hitPoint);
                        }
                        // also slow down time for 2 seconds
                        Time.timeScale = 0.5f;
                        StartCoroutine(ResetTimeScale());
                    }
                    else
                    {
                        // Trigger brick bash response
                        if (brickBarrier != null)
                        {
                            brickBarrier.BrickBashRespond(hitObject, hitPoint);
                        }
                    }
                }
                attacking = false;
                retracting = true;
                // Ignore further hits during retraction/after attack finished
                weaponCollider.isTrigger = true;
            }
        }
    }

    private IEnumerator CameraShake()
    {
        cameraOriginalPosition = playerCamera.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < cameraShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / cameraShakeDuration;
            float intensity = (1f - t) * cameraShakeStrength; // Fade out the shake

            Vector3 randomShake = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * intensity;

            playerCamera.transform.localPosition = cameraOriginalPosition + randomShake;
            yield return null;
        }

        playerCamera.transform.localPosition = cameraOriginalPosition;
    }

    private IEnumerator ResetTimeScale()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
    }
}