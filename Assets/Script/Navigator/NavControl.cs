using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using UnityEngine.Events;
using TMPro;



namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Responsible for controlling navigation using guide bot.
    /// </summary>
    public class NavControl : MonoBehaviour
    {
        /// <summary>
        /// prefab to instantiate and show the navigation.
        /// </summary>
        public GameObject pathPrefab;
        /// <summary>
        /// navigation points to target.
        /// </summary>
        public List<Vector3> allNavPoints;
        /// <summary>
        /// guide bot controller.
        /// </summary>
        public GameObject robot;
       
        /// <summary>
        /// panel that holds Dialogue and Target option.
        /// </summary>
        public GameObject worldSpacePanel;
        /// <summary>
        /// Navigation Panel message.
        /// </summary>
        public TMP_Text navPanelMessage;
        /// <summary>
        /// string used to change navPanelMessage when target reached.
        /// </summary>
        public string reachedMessage;
        /// <summary>
        /// Distance in front of the camera the guide bot will spawn.
        /// </summary>
        public const float DISTANCEFROMCAMERA = 1.1f;
        /// <summary>
        /// Camera transform reference.
        /// </summary>
        private Transform cameraPosition;

        public delegate void PrefabAction(GameObject pathPrefab);
        public static event PrefabAction OnSetPrefab;
        
        private GuideBotController botController;

        public MyEvent myEvent;

        private void Start()
        {
            botController = robot.GetComponent<GuideBotController>();
            cameraPosition = Camera.main.transform;

            //subscribe to event when display path is called positions added to the list we get that info.
            CustomPathRenderer.OnSetListOfPositions += SetListOfPoints; 
            GuideBotController.OnFinishedNavigation += ShowFinishedPanel;
            if (myEvent == null)
                myEvent = new MyEvent();

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
            robot.transform.position = cameraPosition.position + cameraPosition.forward * DISTANCEFROMCAMERA;
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
            worldSpacePanel.SetActive(true);
            navPanelMessage.text = reachedMessage;
        }

        private void SetListOfPoints(List<Vector3> listOfPoints)
        {
            allNavPoints = listOfPoints;
        }
 
    }
    
    public class MyEvent : UnityEvent<NavControl>
    {

    }
}


