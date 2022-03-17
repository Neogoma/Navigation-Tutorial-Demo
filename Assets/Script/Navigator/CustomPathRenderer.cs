using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Responsible for overriding the path displayed when doing navigation. Allows for customization of the generated path to target.
/// </summary>
namespace Neogoma.Stardust.Demo.Navigator
{
    public class CustomPathRenderer : AbstractNonMonoInteractive, IPathRenderer
    {
        private List<GameObject> createdPathObjects = new List<GameObject>();
        private List<Vector3> positionList = new List<Vector3>();
        private Vector3 target;
        private GameObject pathPrefab;

        public delegate void SetListOfPosition(List<Vector3> listOfNavPositions);
        public static event SetListOfPosition OnSetListOfPositions;

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
        /// <param name="allNavigationsPoints"></param>
        public void DisplayPath(List<IOctreeCoordnateObject> allNavigationsPoints)
        {
            ExtendPath(allNavigationsPoints);

            InitPath();

            

            if (OnSetListOfPositions != null)
                OnSetListOfPositions(positionList);

        }
        /// <summary>
        /// Subdivides path into more coordinate points using a custom algorithm getting the middle point between the current and the next coordinate point.
        /// </summary>
        /// <param name="_allNavigationsPoints"></param>
        private void ExtendPath(List<IOctreeCoordnateObject> _allNavigationsPoints)
        {
            Vector3 midpoint; // add more to subdivide even further

            for (int j = 0; j < _allNavigationsPoints.Count; j++)
            {
                if (j < _allNavigationsPoints.Count - 1) // if not the last point
                {
                    //mid point is the distance between the current point vector3 and the next divided by 2, getting the coordinate right in the middle of those 2 points
                    midpoint = (_allNavigationsPoints[j].GetCoordnates() + _allNavigationsPoints[j + 1].GetCoordnates()) / 2f;
                    positionList.Add(_allNavigationsPoints[j].GetCoordnates()); // add the current point
                    positionList.Add(midpoint); //add the resulting middle point coord.
                }
                else if (j == _allNavigationsPoints.Count - 1)
                {
                    positionList.Add(_allNavigationsPoints[j].GetCoordnates()); //on the last point just add it to the list.
                }
            }
            return;
        }

        /// <summary>
        /// Begins Looping through the new and more complex list of coordinate points
        /// Gets direction and position of all points to Instantiate the desired path prefab correctly.
        /// </summary>
        private void InitPath()
        {
            NavigationDemo.OnSetTarget += SetTarget;
            NavControl.OnSetPrefab += SetPrefab;
            for (int i = 0; i < positionList.Count; i++) //looping through the new coordinates with midpoints included.
            {
                Vector3 targetDirection;
                GameObject goInstance;

                //getting the correct direction to rotate each navigation prefab correctly to the next point.
                if (i == positionList.Count - 1) 
                {
                    targetDirection = target - positionList[i]; //last point just face the target
                }
                else
                {
                    targetDirection = positionList[i + 1] - positionList[i]; //face the next point on the list
                }

                Quaternion lookRotation = Quaternion.LookRotation(targetDirection); //store rotation

                if (lookRotation.y < 0) //force objects to point in the right direction if they point backwards in regards to next point on the list.
                {
                    Quaternion inverseLookRotation = Quaternion.Inverse(lookRotation);
                    goInstance = GameObject.Instantiate(pathPrefab, positionList[i], inverseLookRotation);
                }
                else
                {
                    goInstance = GameObject.Instantiate(pathPrefab, positionList[i], lookRotation);
                }

                goInstance.transform.position = new Vector3(positionList[i].x, -1, positionList[i].z); // path is displayed at camera height, so we want to instantiate a bit lower at a ground level.
                createdPathObjects.Add(goInstance.gameObject); //add to the new created path list.
            }
        }

        /// <summary>
        /// Invoked when target is set on NavigationDemo.
        /// </summary>
        /// <param name="_target"> navigation target position</param>
        private void SetTarget(Vector3 _target)
        {
            target = _target;
        }

        /// <summary>
        /// Invoked when prefab set on NavControl.
        /// </summary>
        /// <param name="_pathPrefab"> prefab object to display on path.</param>
        private void SetPrefab(GameObject _pathPrefab)
        {
            pathPrefab = _pathPrefab;
        }
    }
}

