using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class Ball : MonoBehaviour
{
    // Configs
    [Header("Configs")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Color32 landColor;
    [SerializeField] Color32 flyColor;
    bool canShot = true;
    bool isFlying = false;

    // Child Objects
    [Header("Child")]
    [SerializeField] SpriteRenderer body = null;
    [SerializeField] AimLine aimLine = null;

    [Header("MMFeedbacks")]
    [SerializeField] MMFeedbacks shotFeedback = null;
    [SerializeField] MMFeedbacks landFeedback = null;



    // Shot Variables
    Vector2 currentDirection;
    Vector2 collidedPos;

    // Components
    Rigidbody2D myRigidbody2D;
    Collider2D myCollider2D;
    Animator myAnimator;

    // FOR DEBUG
    Vector2 perpendicularLine;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // DrawAim();
        Shot();
    }

    private void FixedUpdate()
    {
        Fly();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Land(other);
    }

    ////////////////////////////////////////////////

    void DrawAim()
    {
        if (canShot)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirection(), Mathf.Infinity,
                LayerMask.GetMask("Ground"));

            aimLine.DrawAimLine(transform.position, hit.point);
        }
    }

    void Shot()
    {
        if (canShot && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A)))
        {
            if(IsInRange())
            {
                canShot = false;
                aimLine.gameObject.SetActive(false);

                isFlying = true;
                currentDirection = GetDirection();

                body.color = flyColor;
                myAnimator.SetTrigger("Fly");
                shotFeedback.PlayFeedbacks();
            }
            else
            {
                myAnimator.SetTrigger("Land");
            }
        }
    }

    private void Fly()
    {
        if (isFlying)
        {
            myRigidbody2D.velocity = currentDirection * Time.deltaTime * moveSpeed;
        }
    }


    void Land(Collision2D other)
    {
        canShot = true;
        aimLine.gameObject.SetActive(true);

        isFlying = false;

        body.color = landColor;
        myAnimator.SetTrigger("Land");
        landFeedback.PlayFeedbacks();

        collidedPos = other.collider.ClosestPoint(transform.position);
        Vector2 directionToLook = (Vector2)transform.position - collidedPos;
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToLook);

        perpendicularLine = Vector3.Normalize(directionToLook);
    }

    Vector2 GetDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0f);

        Vector3 dir = Vector3.Normalize(mousePos - transform.position);

        return dir;
    }

    // return : -180 ~ 180 degree (for unity)
    float GetAngle (Vector2 vStart, Vector2 vEnd)
    {
        Debug.DrawRay(transform.position, vStart);
        Debug.DrawRay(transform.position, vEnd);
        Vector2 v = vEnd - vStart;
 
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    bool IsInRange()
    {
        float shotAngle = Vector2.Angle(GetDirection(), perpendicularLine);

        if(shotAngle >= 90) return false;
        else return true;
    }
}
