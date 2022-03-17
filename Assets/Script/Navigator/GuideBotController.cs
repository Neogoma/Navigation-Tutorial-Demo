using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Neogoma.Stardust.Demo.Navigator
{
    public class GuideBotController : MonoBehaviour
    {
        //List of waypoints to go through.
        public List<Vector3> waypointList = new List<Vector3>();
        //moving speed of  guide bot
        public float moveSpeed;
        //rotation speed of guide bot
        public float rotSpeed;
        //distance from next target to assume guide bot is close enough.
        public float distanceFromTarget;
        //defines if guide bot can move to next point.
        public bool canMove;
        //defines if guide bot can look at player camera.
        public bool canLook;

        private bool isReached1stNode;
        private Vector3 closestTarget;
        private int currIndex = 0;
        private float step;

        public delegate void OnFinishedNav( );
        
        public static event OnFinishedNav OnFinishedNavigation;
        private void Start()
        {
            step = moveSpeed * Time.deltaTime;
        }

        void Update()
        {
            if (canLook)
            {
                RotateToTarget(Camera.main.transform.position);
            }

            if (canMove == true)
            {
                canLook = false;
                FindAndMoveToFirstTarget(closestTarget);
            }
        }

        Vector3 FindClosestTarget()
        {
            Vector3 position = transform.position;
            return waypointList.OrderBy(o => (o - position).sqrMagnitude).FirstOrDefault();
        }

        /// <summary>
        /// Move from spawned position to the nearest point to start navigating.
        /// </summary>
        /// <param name="target"></param>
        public void FindAndMoveToFirstTarget(Vector3 target)
        {

            if (!isReached1stNode)
            {
                RotateToTarget(target);
                if (Vector3.Distance(transform.position, target) <= 0.5f)
                {
                    isReached1stNode = true;
                }
                else
                {
                    Vector3 actualdirection = new Vector3(target.x, transform.position.y, target.z);
                    transform.position = Vector3.MoveTowards(transform.position, actualdirection, step);
                }
            }
            else
            {
                MoveToNextTarget(waypointList);
            }
        }

        /// <summary>
        /// Moves the bot to the next target on the waypoint list until target reached
        /// </summary>
        /// <param name="waypoints"> lis of point positions to move through to get to the target.</param>
        public void MoveToNextTarget(List<Vector3> waypoints)
        {
            if (currIndex <= waypoints.Count - 1)
            {
                RotateToTarget(waypoints[currIndex]);

                if (Vector3.Distance(transform.position, waypoints[currIndex]) <= 0.5f)
                {
                    currIndex++;
                }
                else
                {
                    Vector3 actualdirection = new Vector3(waypoints[currIndex].x, transform.position.y, waypoints[currIndex].z);
                    transform.position = Vector3.MoveTowards(transform.position, actualdirection, step);
                }
            }
            else
            {
                canMove = false;
                canLook = true;
                 
                if (OnFinishedNavigation != null)
                    OnFinishedNavigation();
            }
        }

        /// <summary>
        /// Rotates prefab object to face the target parameter only on 1 axis.
        /// </summary>
        /// <param name="target"> target position </param>
        private void RotateToTarget(Vector3 target)
        {
            Vector3 targetPostition = new Vector3(target.x, transform.position.y, target.z) - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(targetPostition);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Init method takes in navigation points for bot to use.
        /// </summary>
        /// <param name="m_allNavPoints">list of vector3 points of the fastest path to the selected target.</param>
        public void StartNavigation(List<Vector3> m_allNavPoints)
        {
            waypointList = m_allNavPoints;
            waypointList.Reverse();
            canMove = true;
            closestTarget = waypointList[0];
        }
    }
}
