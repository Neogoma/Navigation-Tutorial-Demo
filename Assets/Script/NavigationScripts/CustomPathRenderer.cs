using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


namespace Neogoma.Stardust.Demo.Navigator
{
    public class CustomPathRenderer : AbstractNonMonoInteractive, IPathRenderer
    {
        private List<GameObject> createdPathObjects = new List<GameObject>();
        private List<Vector3> positionList = new List<Vector3>();
        

        /// <summary>
        /// Called by PathFindingManager, clears the current path
        /// </summary>
        public void ClearPath()
        {
            if (createdPathObjects?.Any() == true)
            {
                foreach (GameObject go in createdPathObjects)
                {
                    GameObject.Destroy(go);
                }
            }
        }

        public void DisplayPath(List<IOctreeCoordnateObject> allNavigationsPoints)
        {
            Vector3 midpoint;
           
            for (int j = 0; j < allNavigationsPoints.Count; j++)
            {
                if(j< allNavigationsPoints.Count -1)
                {
                    midpoint = (allNavigationsPoints[j].GetCoordnates() + allNavigationsPoints[j+1].GetCoordnates()) / 2f;
                    positionList.Add(allNavigationsPoints[j].GetCoordnates());
                    positionList.Add(midpoint);
                }
                else if (j == allNavigationsPoints.Count-1 )
                {
                    positionList.Add(allNavigationsPoints[j].GetCoordnates());
                }
            }

            for (int i = 0; i < positionList.Count; i++)
            {
                Vector3 targetDirection;
                GameObject goInstance;
                if (i == positionList.Count -1) // if last point use the target for direction else use use the next point.
                {
                    targetDirection = NavControl.Instance.target - positionList[i];
                }
                else
                {
                    targetDirection = positionList[i + 1] - positionList[i];
                }
               
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
                
                if (lookRotation.y < 0)
                {
                   Quaternion inverseLookRotation = Quaternion.Inverse(lookRotation);
                   goInstance = GameObject.Instantiate(NavControl.Instance.pathPrefab, positionList[i], inverseLookRotation);
                }
                else
                {
                      goInstance = GameObject.Instantiate(NavControl.Instance.pathPrefab, positionList[i], lookRotation);
                }
               
                goInstance.transform.position = new Vector3(positionList[i].x, -1, positionList[i].z);
                createdPathObjects.Add(goInstance.gameObject);
            }
            
            NavControl.Instance.allNavPoints = positionList;
        }
    }
}

