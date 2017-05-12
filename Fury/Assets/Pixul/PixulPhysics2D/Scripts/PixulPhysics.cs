using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fizzyo;

public class PixulPhysics : MonoBehaviour 
{
    private List<GameObject> trajectoryPoints = new List<GameObject>();
    private List<GameObject> oldTrajectoryPoints = new List<GameObject>();
    public GameObject trajectoryPointPrefab;    
    public int noOfTrajectoryPoints = 10;
    public bool enableSolidLine;
    
    public Gradient lineColour;                
    public AnimationCurve widthCurve;
    public int endVerts = 0;    
    public int cornerVerts = 0;
    private LineRenderer m_line;
    public Material lineMaterial;
    public LineTextureMode textureMode;
    public float tileAmount = 1;
    public bool pixulShader;   

    public bool destroyAfterFire = false;
    public float destroyDelay = 1f;

    private bool m_Pressed;
    private bool m_BallThrown;

    public Vector2 m_gravity = new Vector2(0f, -12.0f);
    
    public float m_power = 3f;
    
    public float m_damping = 0.8f;    
    public LayerMask m_ObstaclesLayerMask;

    private float m_accel_rate = 1f;
    private Vector2 m_velocity = new Vector2(0f, 0f);
    private Vector2 m_init_velocity = new Vector2(0f, 0f);
    private float acceleration_rate = 0.0f;
    private float m_acceleration_rate = 0.0f;
    private bool m_hit = false;

    private Vector3 respawn;

    private Vector2 keyVelocity = Vector2.zero;
    public Vector2 badVelocity = new Vector2(1.5f, 1.5f);
    public bool isBad = true;

	// Use this for initialization
	void Start () 
    {
        FizzyoDevice insatnce = FizzyoDevice.Instance();

        this.isBad = true;
        //apply default material
        if (lineMaterial == null)
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
        // Store inital position
        respawn = this.transform.position;


        // spawn trajectoryPoints
        if (trajectoryPointPrefab != null)
        {
            for (int i = 0; i < noOfTrajectoryPoints; i++)
            {
                GameObject dot = (GameObject)Instantiate(trajectoryPointPrefab);
                //dot.tag = "trajectoryPoint";
                dot.GetComponent<Renderer>().enabled = false;
                trajectoryPoints.Insert(i, dot);
            }

            for (int i = 0; i < noOfTrajectoryPoints; i++)
            {
                GameObject dot = (GameObject)Instantiate(trajectoryPointPrefab);
                //dot.tag = "trajectoryPoint";
                dot.GetComponent<Renderer>().enabled = false;
                oldTrajectoryPoints.Insert(i, dot);
            }
        }
        else
        {            
            Debug.LogWarning("Trajectory prefab is null. Please choose one.");
        }               

        // Setup a line renderer
        if ( enableSolidLine )
        {
            GameObject m_line_object = new GameObject("line");
            m_line_object.AddComponent<LineRenderer>();
            m_line = m_line_object.GetComponent<LineRenderer>();
            m_line.numPositions = noOfTrajectoryPoints;           
            m_line.colorGradient = lineColour;            
            m_line.numCornerVertices = cornerVerts;
            m_line.numCapVertices = endVerts;
            m_line.widthCurve = widthCurve;
            m_line.textureMode = textureMode;            
            m_line.material = lineMaterial;
            m_line.material.SetFloat("RepeatX", tileAmount);                                          
        }        
    }
	
	// Update is called once per frame
	void Update () 
    {        
        float pressure = Fizzyo.FizzyoDevice.Instance().Pressure();
        Debug.Log(pressure);

        if (m_BallThrown && (m_velocity.magnitude < 0.25f))
        {
            // Ball has nearly stopped, call death function
            on_death();
        }

        if (Input.GetButtonDown("Fire1") || (Input.GetKey("space") && m_Pressed))
        {
            // Upon mouse released we fire
            fire();

            if (destroyAfterFire)
                if (enableSolidLine)
                    Invoke("DisableSolidLine", destroyDelay);
                else
                    Invoke("DisableDottedLine", destroyDelay);
        }

        if(Input.GetKey("up"))
        {
            keyVelocity += new Vector2(0.04f, 0.02f);

            m_Pressed = true;

            if (enableSolidLine)
                m_line.numPositions = noOfTrajectoryPoints;
            else
                EnableDottedLine();
        } 

        if(Input.GetKey("down"))
        {
            keyVelocity -= new Vector2(0.04f, 0.02f);

            m_Pressed = true;

            if (enableSolidLine)
                m_line.numPositions = noOfTrajectoryPoints;
            else
                EnableDottedLine();
        }
    }

    public void OnBecameInvisible()
    {
        on_death();
    }

    // Call this when you want the ball to die
    private void on_death()
    {
        // Currently this resets the ball position and thrown flag
        transform.position = respawn;
        m_BallThrown = false;

        // Todo: add further death functionality, anim triggers etc
    }

    public void OnMouseDown()
    {
        // Pressed flag
        m_Pressed = true;

        if (enableSolidLine)
            m_line.numPositions = noOfTrajectoryPoints;
        else
            EnableDottedLine();


    }

    // Accessor funtions
    public bool is_pressed()
    {
        return m_Pressed;
    }

    // Call to disable the physics
    public void stop_object()
    {
        m_BallThrown = false;
    }

    // Fire function
    public void fire()
    {
        // This will calculate the required initial velocity based upon the mouseposition
        m_Pressed = false;
        m_BallThrown = true;

        if (this.isBad == true)
        {
            m_init_velocity = keyVelocity - badVelocity;    
        }
        else
        {
            m_init_velocity = keyVelocity;
        }
        //transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_velocity = m_init_velocity * m_power;

        m_acceleration_rate = m_velocity.magnitude * m_accel_rate;

        // Todo: add further fire functionality, anims/effects etc
    }

    void FixedUpdate()
    {
        // Active ( thrown ) ball update
        if (m_BallThrown)
        {
            float play_speed = Time.fixedDeltaTime;

            // Adjust velocity each frame based upon acceleration rate and gravity
            m_velocity += ((m_init_velocity * m_acceleration_rate) * play_speed);
            m_velocity += (m_gravity * play_speed);
            RaycastHit2D[] hit;
            Vector2 last_pos = transform.position;

            // acceleration falloff
            m_acceleration_rate *= (0.5f * play_speed);

            // Establish the layers we will collide with            
            LayerMask layerMask = 1 << m_ObstaclesLayerMask; //bit shift to correctly apply mask            

            // Get the distance covered this frame
            float distance = (last_pos - (last_pos + (m_velocity * play_speed))).magnitude;

            // Establish which type of collider we have and check for collisions over this frames distance
            CircleCollider2D circle = GetComponent<CircleCollider2D>();
            BoxCollider2D box = GetComponent<BoxCollider2D>();

            if (circle != null)
            {
                hit = Physics2D.CircleCastAll(last_pos, circle.radius, m_velocity * play_speed, distance, layerMask.value);
            }
            else if( box != null )
            {
                hit = Physics2D.BoxCastAll(last_pos, box.size, 0f, m_velocity, distance, layerMask.value);
            }
            else
            {
                hit = Physics2D.LinecastAll(last_pos, (last_pos + m_velocity * play_speed), layerMask.value);
            }

            // Hit counter
            int hits = 0;

            // Temp velocity to store all our collisions into
            Vector3 temp_velocity = new Vector3(0f, 0f, 0f);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i] && hit[i].collider != null)
                {
                    if (!m_hit && hit[i] && hit[i].collider != this.GetComponent<Collider2D>())
                    {
                        // Default damping factor for any collision
                        float b_damping = m_damping;

                        CollisionProperties col_prop = hit[i].collider.transform.gameObject.GetComponent<CollisionProperties>();
                        Rigidbody2D rigidbody = hit[i].collider.transform.gameObject.GetComponent<Rigidbody2D>();

                        if (col_prop)
                        {
                            // If the collided object has a CollisionProperties component we can grab the custom damping factor from this
                            b_damping = col_prop.damping;
                        }

                        if (rigidbody)
                        {
                            // we've hit a rigid body, apply a force to his rigidbody based upon our velocity
                            rigidbody.AddForceAtPosition(m_velocity * 10f, last_pos);
                        }
                        
                        // Add each reflection vector into the temp_velocity
                        temp_velocity += Vector3.Reflect(m_velocity * b_damping, hit[i].normal);
                        hits++;
                    }
                }
            }

            if (hits > 0)
            {
                // We've hit something last time, so we stop colliding until we are clear of collisions
                m_hit = true;
            }
            else 
            {
                // We are clear of collisions, reset the hit flag
                m_hit = false;
            }

            // Extra check based upon the colliders array to clear the hit flag ( redundant? )
            if( hit.Length == 0)
            {
                m_hit = false;
            }

            if (m_hit)
            {
                // If we hit we apply an averaged new ( reflected ) velocity based upon the number of collisions we had.
                m_velocity = (temp_velocity / hits);
            }

            // Apply to transform
            Vector3 velocity3 = new Vector3(m_velocity.x, m_velocity.y, 0);
            transform.position += (velocity3 * play_speed);
        }

        // Trajectory generation
        if (m_Pressed )
        {
            // Setup initial parameters based upon the mouse position.
            Vector2 last_pos = gameObject.transform.position;
            Vector2 velocity = keyVelocity;//transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 orignal_velocity = velocity;

            velocity *= m_power;

            acceleration_rate = velocity.magnitude * m_accel_rate;

            float travelled = 0f;

            // Default distance to check between trajectory points
            float dist_to_check = 1f;

            bool we_hit = false;
            bool hit_last_time = false;
            bool dead = false;
            
            int i = 0;

            // Loop through all the available trajectory points
            while (i < noOfTrajectoryPoints)
            {
                float play_speed = Time.fixedDeltaTime;

                // Apply the same velocity calculation which we apply per frame at runtime
                velocity += ((orignal_velocity * acceleration_rate) * play_speed);
                velocity += (m_gravity * play_speed);

                // acceleration falloff
                acceleration_rate *= (0.5f * play_speed);

                // Establish obstacle layermash
                LayerMask layerMask = 1 << m_ObstaclesLayerMask;

                // Distance travelled this loop
                float distance = (last_pos - (last_pos + (velocity * play_speed))).magnitude;

                if (i == 0)
                {
                    // On the first loop set the distance_to_check to the distance we have travelled .
                    // This gives the player a sense of force throughout the entire trajectory.
                    dist_to_check = distance;

                    // Makes the line look nicer
                    dist_to_check *= 5;
                }

                // Increase our travelled variable by the distance this loop
                travelled += distance;

                RaycastHit2D[] hit;

                // Establish which type of collider we have and check for collisions over this frames distance
                CircleCollider2D circle = GetComponent<CircleCollider2D>();
                BoxCollider2D box = GetComponent<BoxCollider2D>();

                if (circle != null)
                {
                    hit = Physics2D.CircleCastAll(last_pos, circle.radius, velocity * play_speed, distance, layerMask.value);
                }
                else if (box != null)
                {
                    hit = Physics2D.BoxCastAll(last_pos, box.size, 0f, velocity, distance, layerMask.value);
                }
                else
                {
                    hit = Physics2D.LinecastAll(last_pos, (last_pos + velocity * play_speed), layerMask.value);
                }

                we_hit = false;
                int hits = 0;
                Vector2 temp_velocity = new Vector2( 0f, 0f );

                for (int j = 0; j < hit.Length; j++)
                {
                    if (!hit_last_time && hit[j] && hit[j].collider != this.GetComponent<Collider2D>())
                    {
                        // Default damping factor for any collision
                        float b_damping = m_damping;

                        CollisionProperties col_prop = hit[j].collider.transform.gameObject.GetComponent<CollisionProperties>();
                        
                        if (col_prop)
                        {
                            // If the collided object has a CollisionProperties component we can grab the custom damping factor from this
                            b_damping = col_prop.damping;
                        }

                        // Add each reflection vector into the temp_velocity
                        temp_velocity += Vector2.Reflect(velocity * b_damping, hit[j].normal);

                        we_hit = true;
                        hits++;
                    }
                }

                if (we_hit)
                {
                    // We've hit something last time, so we stop colliding until we are clear of collisions

                    // If we hit we apply an averaged new ( reflected ) velocity based upon the number of collisions we had.
                    velocity = (temp_velocity / hits);
                    hit_last_time = true;
                }
                else
                {
                    // We've hit something last time, so we stop colliding until we are clear of collisions
                    hit_last_time = false;
                }

                if (velocity.magnitude == 0f)
                {
                    // stop the line since the ball has stopped
                    dead = true;
                }

                // Apply the new velocity to our position
                last_pos += velocity * play_speed;

                // Every time we have travelled further than our distance to check we add a new trajectory point
                if ((travelled > dist_to_check - (dist_to_check * 0.25f)) && i < noOfTrajectoryPoints)
                {
                    // Set the position
                    trajectoryPoints[i].transform.position = last_pos;

                    // Enable the render unless we are using a line renderer or dead
                    if (!enableSolidLine && !dead)
                    {
                        trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                    }
                    else 
                    {
                        trajectoryPoints[i].GetComponent<Renderer>().enabled = false;
                    }

                    // Reset travelled amount
                    travelled = 0f;
                    i++;
                }
            }

            // Now we have all the trajectory points we can calculate the angles to apply between them
            i = 0;
            while (i < noOfTrajectoryPoints)
            {
                int prev_index = i - 1;
                int next_index = i + 1;

                // Check for start and end points
                if (i == noOfTrajectoryPoints - 1)
                {
                    next_index = i;
                }

                if (i == 0)
                {
                    prev_index = i;
                }

                Vector2 next_pos = trajectoryPoints[next_index].transform.position;
                Vector2 old_pos = trajectoryPoints[prev_index].transform.position;

                if (i == 0)
                {
                    // If the start point use the objects position for the old position
                    old_pos = gameObject.transform.position;
                }

                Vector2 newVel = next_pos - old_pos;

                // Calculate and apply the angle to the sprite transform
                float angle = Mathf.Atan2(newVel.y, newVel.x) * Mathf.Rad2Deg;
                trajectoryPoints[i].transform.eulerAngles = new Vector3(0, 0, angle);

                if( !enableSolidLine )
                {
                    trajectoryPoints[i].GetComponent<Renderer>().enabled = true;
                }

                if( enableSolidLine )
                {
                    // line renderer positions
                    Vector3 pos = new Vector3(trajectoryPoints[i].transform.position.x, trajectoryPoints[i].transform.position.y, 10f);
                    m_line.SetPosition(i, pos);
                    m_line.enabled = true;
                }

                i++;
            }
        }
    }

    void DisableSolidLine()
    {        
        m_line.enabled = false;
    }

    void DisableDottedLine()
    {        
        foreach (GameObject go in trajectoryPoints)
        {
            go.SetActive(false);
            go.GetComponent<Renderer>().enabled = false;
        }
        
    }

    void EnableDottedLine()
    {
        foreach (GameObject go in trajectoryPoints)
        {
            go.SetActive(true);
        }

    }
}
