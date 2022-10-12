using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- constants
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string Jump = "Jump";
    [Header("---------- reference -----------")]
    private Transform _transform;
    public CharacterController controller;

    [Header("---------- reference -----------")]
    public Transform viewPoint;
    public float mouseSensitivity = 2f;
    private float verticalRotateStore;
    private Vector2 mouseInput;
    public bool invertLook;
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    private float activeMoveSpeed;
    private Vector3 moveDir;
    private Vector3 movement;
    private Camera cam;
    private Transform camTransform;
    public float jumpForce = 12f;
    public float gravityMod = 2.5f;

    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    
    public Transform bulletImpact;

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        camTransform = cam.transform;
    }

    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw(MouseX), Input.GetAxisRaw(MouseY)) * mouseSensitivity;

        _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y + mouseInput.x, _transform.eulerAngles.z);

        verticalRotateStore += mouseInput.y;

        verticalRotateStore = Mathf.Clamp(verticalRotateStore, -60f, 60f);

        if(invertLook)
        {
            viewPoint.rotation = Quaternion.Euler(verticalRotateStore, viewPoint.eulerAngles.y, viewPoint.eulerAngles.z);
        }
        else
        {
            viewPoint.rotation = Quaternion.Euler(-verticalRotateStore, viewPoint.eulerAngles.y, viewPoint.eulerAngles.z);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        moveDir = new Vector3(Input.GetAxisRaw(Horizontal), 0f, Input.GetAxisRaw(Vertical));

        float yVelocity = movement.y;
        movement = (moveDir.z * _transform.forward + moveDir.x * _transform.right).normalized * activeMoveSpeed;

        if(controller.isGrounded)
        {
            movement.y = 0;
        }
        else
        {
            movement.y = yVelocity;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayer);

        if(Input.GetButtonDown(Jump) && isGrounded)
        {
            movement.y =  jumpForce;
        }

        

        movement.y +=  Physics.gravity.y * Time.deltaTime * gravityMod;
        controller.Move(movement * Time.deltaTime);


        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Cursor.lockState == CursorLockMode.None)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        ray.origin = camTransform.position;

        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);

            Transform obj = Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
            Destroy(obj.gameObject, 10f);
        }
    }

    private void LateUpdate()
    {
        camTransform.position = viewPoint.position;
        camTransform.rotation = viewPoint.rotation;
    }
}
