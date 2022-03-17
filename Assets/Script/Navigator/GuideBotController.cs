using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Controls guide bot movement and rotation throught the defined path to target.
    /// </summary>
    public class GuideBotController : MonoBehaviour
    {
        /// <summary>
        /// rotation speed of guide bot
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// rotation speed of guide bot
        /// </summary>
        public float rotSpeed;
        /// <summary>
        /// rotation speed of guide bot
        /// </summary>
        private List<Vector3> waypointList = new List<Vector3>();
        /// <summary>
        /// defines if guide bot can move to next point.
        /// </summary>
        private bool canMove;
        /// <summary>
        /// defines if guide bot can look at player camera.
        /// </summary>
        private bool canLook;
        /// <summary>
        /// bool flags when first node on the list has been reached, so we can start to move to other nodes from that.
        /// </summary>
        private bool hasReachedFirstNode;
        /// <summary>
        /// first node position
        /// </summary>
        private Vector3 firstNode;
        /// <summary>
        /// keeps track of current index value.
        /// </summary>
        private int currIndex = 0;
        /// <summary>
        /// this is the speed + time.delta of which the robot will move.
        /// </summary>
        private float step;
        /// <summary>
        /// threshold distance limit to move to the next coordinate point.
        /// </summary>
        private const float THRESHOLD = 0.5f;
        public delegate void OnFinishedNav( );
        public static event OnFinishedNav OnFinishedNavigation;


        private void Start()
        {
            step = moveSpeed * Time.deltaTime;
        }

        private void Update()
        {
            if (canLook)
            {
                RotateToTarget(Camera.main.transform.position);
            }

            if (canMove)
            {
                canLook = false;
                FindAndMoveToFirstTarget(firstNode);
            }
        }

        /// <summary>
        /// Move from spawned position to the nearest point to start navigating.
        /// </summary>
        /// <param name="target"></param>
        public void FindAndMoveToFirstTarget(Vector3 target)
        {
            if (!hasReachedFirstNode)
            {
                RotateToTarget(target);
                if (Vector3.Distance(transform.position, target) <= THRESHOLD)
                {
                    hasReachedFirstNode = true;
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

                if (Vector3.Distance(transform.position, waypoints[currIndex]) <= THRESHOLD)
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
            firstNode = waypointList[0];
        }
    }
}
