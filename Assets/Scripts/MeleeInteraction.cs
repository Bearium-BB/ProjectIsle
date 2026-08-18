using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeInteraction : MonoBehaviour
{
    [SerializeField] private int raycastCount = 5;
    [SerializeField] private float separationDegrees = 10f;
    [SerializeField] private float raycastDistance = 5f;

    private InputSystem_Actions inputActions;

    public Inventory inventory;

    private void Awake()
    {
        //Debug.Log("Input actions created");

        inputActions = new InputSystem_Actions();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private RaycastHit? CastRayCone()
    {
        int centerIndex = raycastCount / 2;

        for (int i = 0; i < raycastCount; i++)
        {
            float angle = (i - centerIndex) * separationDegrees;

            Vector3 direction =
                Quaternion.Euler(0f, angle, 0f) * transform.forward;

            Debug.DrawRay(
                transform.position,
                direction * raycastDistance,
                Color.red
            );

            if (Physics.Raycast(
                transform.position,
                direction,
                out RaycastHit hit,
                raycastDistance))
            {
                return hit;
            }
        }

        return null;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        //Debug.Log("Input actions enabled");

        inputActions.Player.Attack.performed += OnInteract;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnInteract;

        inputActions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        RaycastHit? hit = CastRayCone();

        if (hit != null)
        {
            if (hit.Value.collider.gameObject.transform.TryGetComponent(out DropList drops))
            {
                drops.ProcessDropList();
                foreach (Item item in drops.ProcessDropList())
                {
                    inventory.AddInventory(item , 1);
                }
            }
        }
        //Debug.Log("Interact pressed!");
    }
}
