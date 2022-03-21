using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Responsible for overriding the path displayed when doing navigation. Allows for customization of the generated path to target.
    /// </summary>
    public class CustomPathRenderer : AbstractNonMonoInteractive, IPathRenderer
    {
        /// <summary>
        /// List of path objects created to display the path
        /// </summary>
        private List<GameObject> createdPathObjects = new List<GameObject>();
        /// <summary>
        /// list of new coordinate points after subdividing the path
        /// </summary>
        private List<Vector3> positionList = new List<Vector3>();
        /// <summary>
        /// target position
        /// </summary>
        private Vector3 target;
        /// <summary>
        /// prefab Object to display on the path. 
        /// </summary>
        private GameObject pathPrefab;
        /// <summary>
        /// Called when new list of points is calculated.
        /// </summary>
        public UnityEvent<List<Vector3>> OnCalculatedPointList = new UnityEvent<List<Vector3>>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathPrefab">path prefab gameObject</param>
        public CustomPathRenderer(GameObject pathPrefab)
        {
            this.pathPrefab = pathPrefab;
        }

        /// <summary>
        /// Called by PathFindingManager, clears the current path
        /// </summary>
        public void ClearPath()
        {
            foreach (GameObject go in createdPathObjects)
            {
                GameObject.Destroy(go);
            }
            createdPathObjects.Clear();
            positionList.Clear();
        }

        /// <summary>
        /// Displays the path following the given list of coordinate points in space. For a path with more subdivided points, add more midpoints to the list.
        /// </summary>
        /// <param name="allNavigationsPoints">Navigation point list calculated in PathFindingManager.cs </param>
        public void DisplayPath(List<IOctreeCoordinateObject> allNavigationsPoints)
        {
            ExtendPath(allNavigationsPoints);
            InitPath();
            OnCalculatedPointList.Invoke(positionList);
        }

        /// <summary>
        /// Subdivides path into more coordinate points using a custom algorithm getting the middle point between the current and the next coordinate point.
        /// </summary>
        /// <param name="allNavigationsPointsList">Calculated List of Navigation points to raeach the selected target.</param>
        private void ExtendPath(List<IOctreeCoordinateObject> allNavigationsPointsList)
        {
            // add more midpoints to subdivide the path even further
            Vector3 midpoint; 

            for (int j = 0; j < allNavigationsPointsList.Count; j++)
            {
                // if not the last point
                if (j < allNavigationsPointsList.Count - 1) 
                {
                    //mid point is the distance between the current point vector3 and the next divided by 2, getting the coordinate right in the middle of those 2 points.
                    midpoint = (allNavigationsPointsList[j].GetCoordinates() + allNavigationsPointsList[j + 1].GetCoordinates()) / 2f;
                    // add the current point.
                    positionList.Add(allNavigationsPointsList[j].GetCoordinates());
                    //add the resulting middle point coord.
                    positionList.Add(midpoint); 
                }
                else if (j == allNavigationsPointsList.Count - 1)
                {
                    //on the last point just add it to the list.
                    positionList.Add(allNavigationsPointsList[j].GetCoordinates());
                }
            }
            return;
        }

        /// <summary>
        /// Begins Looping through the new and more complex list of coordinate points.
        /// Gets direction and position of all points to Instantiate the desired path prefab correctly.
        /// </summary>
        private void InitPath()
        {
            Quaternion lookRotation;
            Vector3 targetDirection;
            GameObject goInstance;
            //looping through the new coordinates with midpoints included.
            for (int i = 0; i < positionList.Count; i++) 
            {
                //getting the correct direction to rotate each navigation prefab correctly to the next point.
                if (i == positionList.Count - 1) 
                {
                    //last point just face the target.
                    targetDirection = target - positionList[i]; 
                }
                else
                {
                    //face the next point on the list.
                    targetDirection = positionList[i + 1] - positionList[i]; 
                }

                //store rotation.
                lookRotation = Quaternion.LookRotation(targetDirection); 

                //force objects to point in the right direction if they point backwards in regards to next point on the list.
                if (lookRotation.y < 0) 
                {
                    lookRotation = Quaternion.Inverse(lookRotation);
                }
                goInstance = GameObject.Instantiate(pathPrefab, positionList[i], lookRotation);
                // path is displayed at camera height, so we want to instantiate a bit lower at a ground level.
                goInstance.transform.position = new Vector3(positionList[i].x, -1, positionList[i].z);
                //add to the new created path list.
                createdPathObjects.Add(goInstance.gameObject);
            }
        }

        /// <summary>
        /// Invoked when target is set on NavigationDemo.
        /// </summary>
        /// <param name="target"> navigation target position.</param>
        public void SetTarget(Vector3 target)
        {
            this.target = target;
        }

    }
}

