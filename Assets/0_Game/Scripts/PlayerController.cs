using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---------- reference -----------")]
    private Transform _transform;

    [Header("---------- reference -----------")]
    public Transform viewPoint;
    public float mouseSensitivity = 2f;
    private float verticalRotateStore;
    private Vector2 mouseInput;

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {

    }

    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        _transform.rotation = Quaternion.Euler(_transform.eulerAngles.x, _transform.eulerAngles.y + mouseInput.x, _transform.eulerAngles.z);

        verticalRotateStore -= mouseInput.y;

        verticalRotateStore = Mathf.Clamp(verticalRotateStore, -60f, 60f);

        viewPoint.rotation = Quaternion.Euler(verticalRotateStore, viewPoint.eulerAngles.y, viewPoint.eulerAngles.z);
    }
}
