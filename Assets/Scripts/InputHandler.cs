using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerController playerController;

    public InputActions playerControls;
    InputAction moveAction, lookAction, hitAction, jumpAction;

    void Awake()
    {
       playerControls = new InputActions();
       playerController = FindFirstObjectByType<PlayerController>();
    }

    void OnEnable()
    {
        playerControls.Enable();
        moveAction = playerControls.Player.Move;
        lookAction = playerControls.Player.Look;
        hitAction = playerControls.Player.Attack;
        jumpAction = playerControls.Player.Jump;
        hitAction.performed += AttackAction;
        jumpAction.performed += JumpAction;
    }
    void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        playerController.Move(moveVector);

        // Handle look input
        Vector2 lookVector = lookAction.ReadValue<Vector2>();
        playerController.Rotate(lookVector);
    }

    void AttackAction(InputAction.CallbackContext context)
    {
        playerController.Attack();
    }

    void JumpAction(InputAction.CallbackContext context)
    {
        playerController.Jump();
    }
}
