using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float dodgeForce;
    [SerializeField] LayerMask layerMask;
    private float jumpCount = 0;
    private bool canDodge = true;
    bool isGrounded = true;


    [SerializeField]private float cooldownTime = 1.5f;
    private float nextDashTime = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        dodge();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;



    }


    void dodge()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {

            Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
            float dirForce = Mathf.Abs(moveDir.normalized.x * dodgeForce);
            Vector2 transformTo = new Vector2(dirForce, 0);

            // Kollar om det finns en vägg.
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, moveDir, transformTo.x, layerMask);

            if (raycastHit2D.collider != null)
            {
                transform.position = new Vector3(raycastHit2D.point.x + (-moveDir.x * gameObject.GetComponent<CircleCollider2D>().bounds.size.x * 0.5f), transform.position.y, 0);

            }
            else
            {

                transform.position += new Vector3(transformTo.x * moveDir.x, 0, 0);

            }


            nextDashTime = Time.time + cooldownTime;




        }


    }




    void Jump()
    {
       /* if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < 2))
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            jumpCount++;
        }
       */
        if (Input.GetButtonDown("Jump") && (jumpCount < 2))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
         
            jumpCount++;
        }

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
        jumpCount = 0;


    }
}
