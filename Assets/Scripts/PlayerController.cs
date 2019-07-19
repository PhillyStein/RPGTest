using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D theRB;

    public float moveSpeed;

    private Animator anim;

    private bool playerMoving;
    private Vector2 lastMove;

    public string fromLayer;
    public string newLayer;
    private SpriteRenderer sprite;

    public static PlayerController instance;

    public string areaTransitionName;

    public LayerController lc;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        anim = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        }
        else
        {
            theRB.velocity = Vector2.zero;
        }
        playerMoving = false;

        anim.SetFloat("MoveX", theRB.velocity.x);
        anim.SetFloat("MoveY", theRB.velocity.y);

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f || Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            if (canMove)
            {
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                playerMoving = true;
            }
        }

        anim.SetBool("PlayerMoving", playerMoving);

        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LayerTrigger")
        {
            lc = other.GetComponent<LayerController>();
            sprite.sortingLayerName = lc.thisLayer;
        }
    }

    public void SetBounds(Vector3 bottomLeft, Vector3 topRight)
    {
        bottomLeftLimit = bottomLeft + new Vector3(1f, 1.5f, 0f);
        topRightLimit = topRight + new Vector3(-1f, -1.5f, 0f);
    }

}
