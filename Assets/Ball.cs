using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configs
    [SerializeField] float moveSpeed = 3f;
    bool canFly = true;
    bool isFlying = false;

    // Shot
    Vector2 currentDirection;

    // Components
    Rigidbody2D rigidbody2D;
    Collider2D collider2D;
    Animator animator;
    

    private void Awake() 
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();   
    }

    private void Update() 
    {
        HandleRay();    
        Shot();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        canFly = true;
        isFlying = false;

        animator.SetBool("isFlying", false);
    }

    void HandleRay()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (mousePos - transform.position) * 100f;

        Physics2D.Raycast(transform.position, dir);
        Debug.DrawRay(transform.position, dir);
    }

    void Shot()
    {
        if(canFly && Input.GetMouseButtonDown(0))
        {
            canFly = false;
            isFlying = true;
            animator.SetBool("isFlying", true);
            SetDirection();
        }

        if(isFlying)
        {
            rigidbody2D.velocity = currentDirection * Time.deltaTime * moveSpeed;
        }
    }

    void SetDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0f);

        Vector3 dir = Vector3.Normalize(mousePos - transform.position);
        
        // print("dir: " + dir);
        print("mag: " + Vector3.Magnitude(dir));
        

        currentDirection = dir;
    }
}
