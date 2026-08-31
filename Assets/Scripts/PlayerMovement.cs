using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private InputSystem_Actions inputActions;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        inputActions = new InputSystem_Actions();

        // Automatically find the main camera if one wasn't assigned
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        // Get camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Remove vertical movement from the camera
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convert input into camera-relative movement
        Vector3 direction =
            cameraForward * input.y +
            cameraRight * input.x;

        // Don't rotate if we're not moving
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Move
        controller.Move(
            direction.normalized *
            moveSpeed *
            Time.deltaTime
        );
    }
}
