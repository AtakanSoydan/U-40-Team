using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler _input;

    public Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool rotateTowardsMouse;

    [SerializeField] private Camera _camera;

    Transform cam;
    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;

    float forwardAmount;
    float turnAmount;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
        SetupAnimator();

        cam = Camera.main.transform;
    }

    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        var movementVector = MoveTowardTarget(targetVector);
        if (!rotateTowardsMouse)
            RotateTowardMovementVector(movementVector);
        else
            rotateTowardsMouseVector();
    }

    private void FixedUpdate()
    {
        if(cam != null)
        {
            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1).normalized);
            move = _input.v * camForward + _input.h * cam.right;
        }
        else
        {
            move = _input.v * Vector3.forward + _input.h * Vector3.right;
        }

        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        Move(move);
    }

    void Move(Vector3 move)
    { 
        if(move.magnitude < 1)
        {
            move.Normalize();
        }

        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = localMove.x;

        forwardAmount = localMove.z;
    }
    void UpdateAnimator()
    {
        anim.SetFloat("vertical", forwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("horizontal", turnAmount, 0.1f, Time.deltaTime);
    }

    private void rotateTowardsMouseVector()
    {
        Ray ray =_camera.ScreenPointToRay(_input.MousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: 300f))
        {
            var target = hitInfo.point;
            target.y = transform.position.y;
            Vector3 targetVector = new(target.x, transform.position.y, target.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetVector - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if(movementVector.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;

        targetVector = Quaternion.Euler(0, _camera.gameObject.transform.eulerAngles.y, 0) * targetVector;
        var targetPosition = transform.position + targetVector * speed;
        targetVector = Vector3.Normalize(targetVector);
        transform.position = targetPosition;

        return targetVector;
    }

    void SetupAnimator()
    {
        anim = GetComponent<Animator>();

        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if(childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
}
