using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Distance")]
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 3f;

    [Header("Starting Rotation")]
    [SerializeField] private float startingX = 30f;
    [SerializeField] private float startingY = 0f;
    [SerializeField] private float startingZ = 0f;

    [Header("Mouse Orbit")]
    [SerializeField] private float mouseSensitivity = 0.2f;

    [SerializeField] private bool rotateX = true;
    [SerializeField] private bool rotateY = true;
    [SerializeField] private bool rotateZ = false;

    [Header("Vertical Limits")]
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Following")]
    [SerializeField] private float followSpeed = 10f;

    private float rotationX;
    private float rotationY;
    private float rotationZ;

    private float lastStartingX;
    private float lastStartingY;
    private float lastStartingZ;

    private void Start()
    {
        ApplyStartingRotation();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Detect changes made to the Starting Rotation
        // in the Inspector while the game is running.
        if (startingX != lastStartingX ||
            startingY != lastStartingY ||
            startingZ != lastStartingZ)
        {
            ApplyStartingRotation();
        }

        // Hold Middle Mouse Button to orbit
        if (Mouse.current != null &&
            Mouse.current.middleButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            if (rotateX)
            {
                rotationX -= mouseDelta.y * mouseSensitivity;

                rotationX = Mathf.Clamp(
                    rotationX,
                    minPitch,
                    maxPitch
                );
            }

            if (rotateY)
            {
                rotationY += mouseDelta.x * mouseSensitivity;
            }
        }

        Quaternion orbitRotation = Quaternion.Euler(
            rotationX,
            rotationY,
            rotateZ ? rotationZ : 0f
        );

        Vector3 targetPosition =
            target.position + Vector3.up * height;

        Vector3 desiredPosition =
            targetPosition -
            orbitRotation * Vector3.forward * distance;

        transform.position = desiredPosition;

        Quaternion desiredRotation = Quaternion.LookRotation(
            targetPosition - transform.position
        );

        transform.rotation = desiredRotation;
    }

    private void ApplyStartingRotation()
    {
        rotationX = startingX;
        rotationY = startingY;
        rotationZ = startingZ;

        lastStartingX = startingX;
        lastStartingY = startingY;
        lastStartingZ = startingZ;
    }
}
