using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Responsible for controlling navigation using guide bot.
/// </summary>

namespace Neogoma.Stardust.Demo.Navigator
{
    public class NavControl : MonoBehaviour
    {
        //prefab to instantiate and show the navigation.
        public GameObject pathPrefab;
        //navigation points to target.
        public List<Vector3> allNavPoints;
        //guide bot controller.
        public GameObject robot;
        //reference to target position.
        [HideInInspector]
        public Vector3 target;
        //Opens Up compass panel with arrow.
        public GameObject CompassButton;
        //panel that holds Dialogue and Target option.
        public GameObject worldSpacePanel;
        //Navigation Panel message.
        public TMP_Text navPanelMessage;
        //string used to change navPanelMessage when target reached.
        public string reachedMessage;
        //compass arrow controller component.
        public ArrowController arrowController;

        public delegate void PrefabAction(GameObject pathPrefab);
        public static event PrefabAction OnSetPrefab;

        private Transform cameraPosition;
        private GuideBotController botController;
        
       
 
        private void Start()
        {
            botController = robot.GetComponent<GuideBotController>();
            cameraPosition = Camera.main.transform;
             
            CustomPathRenderer.OnSetListOfPositions += SetListOfPoints; //subscribe to event when display path is called positions added to the list we get that info.
            GuideBotController.OnFinishedNavigation += ShowFinishedPanel;

            if (pathPrefab != null)
            {
                if (OnSetPrefab != null)
                    OnSetPrefab(pathPrefab);
            }
            else
            {
                Debug.Log("No Prefab attached to display path");
            }

        }

        /// <summary>
        /// //Initializes navigation for navigation bot
        /// </summary>
        public void InitBot()
        {
            worldSpacePanel.gameObject.SetActive(true);
            robot.gameObject.SetActive(true);
            robot.transform.position = cameraPosition.position + cameraPosition.forward * 1.1f;
            Vector3 targetPostition = new Vector3(cameraPosition.position.x, transform.position.y, cameraPosition.position.z) - transform.position;
            robot.transform.LookAt(targetPostition);
        }

        /// <summary>
        /// Starts navigation on GuideBotController component.
        /// </summary>
        public void StartMoving()
        {
            botController.StartNavigation(allNavPoints);
        }

        /// <summary>
        /// Shows UI panel in worldspace to choose next target witg a default message.
        /// </summary>
        private void ShowFinishedPanel()
        {
            //enable panel with default message where would you like to go.
            worldSpacePanel.SetActive(true);
            navPanelMessage.text = reachedMessage;
        }

        private void SetListOfPoints(List<Vector3> listOfPoints)
        {
            allNavPoints = listOfPoints;
        }
 
    }
}
