using UnityEngine;

public class FollowThePath : MonoBehaviour {

    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

    [HideInInspector]
    public int moveD;

	// Use this for initialization
	private void Start () {
        moveD=1;
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	private void Update () {
        if (moveAllowed)
            Move();
	}

    public void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);
            

            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                
               // if(waypointIndex==10)
                 //   moveD=-moveD;
               
               if(waypointIndex>0||moveD==1)
               waypointIndex+=moveD;
               if(waypointIndex==0&&moveD==-1)
               moveD=1;
            }
        }
    }
}
