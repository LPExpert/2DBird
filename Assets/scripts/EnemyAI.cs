using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    Transform target;
    public Transform targetPatrolPoint1;
    public Transform targetPatrolPoint2;
    public Transform targetPlayer;
    Transform[] patrolPoint;
    int currentPatrolPoint;

    float playerDistance;
    public float aggroDistance;

    public float speed = 400f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        patrolPoint = new Transform[] { targetPatrolPoint1, targetPatrolPoint2 };
        currentPatrolPoint = 0;
        target = targetPatrolPoint1;
        playerDistance = Vector2.Distance(targetPlayer.position, rb.position);

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        //Debug.Log(path.vectorPath[currentWaypoint + 1]);
        //Debug.Log(path.vectorPath.Count + "d");
        //Debug.Log(currentWaypoint + "w");
        //Debug.Log((Vector2)path.vectorPath[currentWaypoint] - rb.position);
        playerDistance = Vector2.Distance(targetPlayer.position,rb.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target.position.Equals(patrolPoint[currentPatrolPoint].position) && reachedEndOfPath)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoint.Length;
            target = patrolPoint[currentPatrolPoint];
            reachedEndOfPath = false;
            currentWaypoint = 0;
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            return;
        }

        if(playerDistance < aggroDistance && !target.position.Equals(targetPlayer.position))
        {
            target = targetPlayer;
            currentWaypoint = 0;
            return;
        }

        if (playerDistance > aggroDistance * 1.5f && target.position.Equals(targetPlayer.position))
        {
            target = patrolPoint[currentPatrolPoint];
            currentWaypoint = 0;
            return;
        }

        if (path == null)
            return;


        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            if(!target.position.Equals(targetPlayer.position))
                CancelInvoke();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        if(Mathf.Pow(direction.x,2f) < 1f && Mathf.Pow(direction.x, 2f) > 0)
        {
            direction.x = (1 / direction.x) * Mathf.Abs(direction.x);
        }

        if (Mathf.Pow(direction.y, 2f) < 1f && Mathf.Pow(direction.y, 2f) > 0)
        {
            direction.y = (1 / direction.y) * Mathf.Abs(direction.y);
        }

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
