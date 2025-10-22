

using UnityEngine;
/* if you are wondering what this code is , this is a simple enemy Patrolling system where 
 enemy identifies the cliff(where there is no ground )or wall and change its direction(flips) and do the patrolling */

//enemy look for cliff and wall using raycast.

public class  EnemyMovement: MonoBehaviour 
{
    public Transform groundCheck;   // i really don't like this name.
                                    // basically from this transform the raycast for ground checking is done.
    public Transform wallcheck;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] 
    private LayerMask wallLayer;
    public float rayDistance = 1f;   //how much the ray should be casted.

    /* when we cast the ray for checking wall or ground we need to store the rayhited information */
    private RaycastHit2D wallHit;
    private RaycastHit2D groundHit;


    //Things that related to enemy movement
    public float speed;
    private int dirIndex = 1; //right side movement;
    private float xScale;
    public float TimeScan = 1;
    private void Awake()
    {
        xScale = transform.localScale.x;
        
    }
    

    private void Update()
    {
        
        Move();
        groundHit = Physics2D.Raycast(groundCheck.position,Vector2.down,rayDistance,groundLayer);
        if (groundHit.collider != null)
        {
            
        }    
        else
        {
            
            Flip();
        }

        wallHit = Physics2D.Raycast(wallcheck.position, Vector2.right, rayDistance, wallLayer);
        if(wallHit.collider != null)
        {
            if (wallHit.collider.CompareTag("Wall"))
            {
                Flip();
            }
            else if (wallHit.collider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = wallHit.collider.GetComponent<PlayerHealth>();
                playerHealth.Die();
                
            }

        }
    }

    

    void Flip()
    {   

        dirIndex *= -1;
        float newX = xScale * dirIndex;
        transform.localScale = new Vector3( newX ,transform.localScale.y,transform.localScale.z);   

    }

    void Move()
    {
        transform.position +=  new Vector3( dirIndex * speed * Time.deltaTime,0f,0f);
    }
    




    //for debugging
    //this function is so cool. when we select the gameObject in the sceneview. it shows the things coded.
    private void OnDrawGizmosSelected()
    {
        Color rayColor = Color.red;
        Debug.DrawRay(groundCheck.position, Vector2.down * rayDistance,rayColor);
        Debug.DrawRay(wallcheck.position, Vector2.right * rayDistance,rayColor);
    }
}

