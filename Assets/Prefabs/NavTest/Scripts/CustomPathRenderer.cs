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
        
        public List<GameObject> createdPathObjects = new List<GameObject>();
        public GameObject goToSpawn;
        public void ClearPath()
        {
            //Write what needs to be done here to clear the path


            bool isNullOrEmpty = createdPathObjects?.Any() != true;
            if (!isNullOrEmpty)
            {
                foreach (GameObject go in createdPathObjects)
                {
                    GameObject.Destroy(go);
                    Debug.Log("Clearing Path....");
                }
            }
            Debug.Log("Clear Path");
        }

        public void DisplayPath(List<IOctreeCoordnateObject> allNavigationsPoints)
        {
            NavControl.Instance.allNavPoints = allNavigationsPoints;
            //Write here to display the path
            foreach (IOctreeCoordnateObject point in allNavigationsPoints)
            {
                GameObject goInstance = GameObject.Instantiate(NavControl.Instance.pathPrefab);

                goInstance.transform.localScale = new Vector3(1f, 1f, 1f);
                goInstance.transform.position = point.GetCoordnates();
                createdPathObjects.Add(goInstance.gameObject);
                Vector3 coord = point.GetCoordnates();
                Debug.Log("point" + allNavigationsPoints.IndexOf(point)+" With Coords: "+ coord);
            }
        }
    }
}

