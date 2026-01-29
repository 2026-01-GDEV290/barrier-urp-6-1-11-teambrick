using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandlerOld : MonoBehaviour
{
    public PlayerControllerPrimitive playerController;

    public InputActions playerControls;
    InputAction moveAction, lookAction, hitAction;

    void Awake()
    {
       playerControls = new InputActions();
       playerController = FindFirstObjectByType<PlayerControllerPrimitive>();
    }

    void OnEnable()
    {
        playerControls.Enable();
        moveAction = playerControls.Player.Move;
        lookAction = playerControls.Player.Look;
        hitAction = playerControls.Player.Attack;
        hitAction.performed += AttackAction;
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
}
