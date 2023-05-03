using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public float speed;

    private float inputX;

    private float inputY;

    private bool isMoving;

    private Animator[] animators;

    private Vector2 movementInput;

    private bool inputDisable;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        animators = GetComponentsInChildren<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.BeforSceneUnLoadEvent += OnBeforSceneUnLoadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPositon += OnMoveToPositon;
        EventHandler.MouseClickEvent += OnMouseClickEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforSceneUnLoadEvent -= OnBeforSceneUnLoadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPositon -= OnMoveToPositon;
        EventHandler.MouseClickEvent -= OnMouseClickEvent;
    }
    private void Update() 
    {
        if (!inputDisable)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }

    private void FixedUpdate() {
        if(!inputDisable)
         Movement();
    }

    private void PlayerInput()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if(inputX != 0 && inputY != 0)
        {
            inputX *= 0.6f;
            inputY *= 0.6f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            inputX *= 0.5f;
            inputY *= 0.5f;
        }
        movementInput = new Vector2(inputX, inputY);
        
        isMoving = movementInput != Vector2.zero;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed *Time.deltaTime);
    }

    private void SwitchAnimation()
    {
        foreach (var anim in animators)
        {
            anim.SetBool("isMoving",isMoving);
            if (isMoving)
            {
                anim.SetFloat("InputX", inputX);
                anim.SetFloat("InputY", inputY);
            }
        }
    }

    private void OnMouseClickEvent(Vector3 pos, ItemDetails itemDetails)
    {
        EventHandler.CallExcuteActionAfterAnimation(pos, itemDetails);
    }

    private void OnMoveToPositon(Vector3 targetPos)
    {
        transform.position = targetPos;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;
    }

    private void OnBeforSceneUnLoadEvent()
    {
        inputDisable = true;
    }
}
