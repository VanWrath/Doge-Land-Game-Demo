using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float gravityModifier = 1f;
    public float minGroundNormalY = 0.65f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity ();
	}

    protected virtual void ComputeVelocity() 
	{
		
	}

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
		grounded = false;
        
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;
        Movement(move, false);		//horizontal movement
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);		//vertical movement

    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if(distance > minMoveDistance)	//checks for collisions if moving more than a threshold
        {
			
	    int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);		//check for collisions
            hitBufferList.Clear();									//clear old list of hit buffer

	//add new collisions into hit buffer.
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            //check the normals of the hitcast2D list and compare it to a min value
            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
				//determines if character is grounded.
				if (currentNormal.y > minGroundNormalY) {
					grounded = true;
					if (yMovement) {
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

                float projection = Vector2.Dot(velocity, currentNormal); //getting the difference of the velocity and the current normal to determine if we need to reduce the velocity so object doesnt pass through another object.
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }
}
