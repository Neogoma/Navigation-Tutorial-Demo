using com.Neogoma.Stardust.API.Relocation;
using com.Neogoma.Stardust.Datamodel;
using com.Neogoma.Stardust.Graph;
using com.Neogoma.Stardust.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Neogoma.Stardust.Demo.Navigator
{
    /// <summary>
    /// Demo for a navigation use case.
    /// </summary>
    public class NavigationDemo:MonoBehaviour
    {
        /// <summary>
        /// Dropdown to select the targets.
        /// </summary>
        public Dropdown targetSelectionDropDown;
        /// <summary>
        /// Prefab to display on the navigation place.
        /// </summary>
        public GameObject locationPrefab;
        /// <summary>
        /// The target reached hint.
        /// </summary>
        public GameObject targetReachedHint;
        /// <summary>
        /// Event called when target reached.
        /// </summary>
        public UnityEvent targetReached = new UnityEvent();
        /// <summary>
        /// button to enable navigation and guide bot.
        /// </summary>
        public GameObject navigationButton;
        /// <summary>
        /// prefab to instantiate and show the navigation.
        /// </summary>
        public GameObject pathPrefab;
        /// <summary>
        /// GuideBot controller component.
        /// </summary>
        public GuideBotController guideBotController;
        /// <summary>
        /// Arrow controller component.
        /// </summary>
        public ArrowController arrowController;
        /// <summary>
        /// Target List.
        /// </summary>
        public List<ITarget> targets;
        /// <summary>
        /// location prefab.
        /// </summary>
        private GameObject locationInstance;
        /// <summary>
        /// path Finding Manager, responsible for calculating the path to target.
        /// </summary>
        private PathFindingManager pathfindingManager;
        /// <summary>
        /// index of selected target.
        /// </summary>
        private int selectedTargetIndex;
        /// <summary>
        /// Collection of targets and matching index.
        /// </summary>
        private Dictionary<int, ITarget> indexToTarget = new Dictionary<int, ITarget>();
        /// <summary>
        /// Main Camera transform.
        /// </summary>
        private Transform mainCameraTransform;
        /// <summary>
        /// Custom Path Renderer: overrides the way the path is displayed.
        /// </summary>
        private CustomPathRenderer pathRenderer;

        private void Start()
        {
            mainCameraTransform = Camera.main.transform;
            pathfindingManager = PathFindingManager.Instance;
            pathfindingManager.onNavigationDatasReady.AddListener(PathFindingReady);            
            targetSelectionDropDown.onValueChanged.AddListener(OnTargetSelected);
            MapRelocationManager.Instance.onPositionFound.AddListener(PositionFound);
            pathRenderer = new CustomPathRenderer(pathPrefab);
            pathRenderer.OnCalculatedPointList.AddListener(guideBotController.SetWaypointsList);
            PathFindingManager.Instance.SetPathRenderer(pathRenderer);
        }

        /// <summary>
        /// Called when position found event is triggered.
        /// </summary>
        /// <param name="arg0">Relocation results.</param>
        /// <param name="arg1"> Coordinate System.</param>
        private void PositionFound(RelocationResults arg0, CoordinateSystem arg1)
        {
            navigationButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// Enables the target selection dropdown.
        /// </summary>
        public void EnableTargetSelection()
        {
            targetSelectionDropDown.gameObject.SetActive(targets.Count > 0);
        }

        /// <summary>
        /// Will go to the target selected in the dropdown.
        /// </summary>
        public void GoToSelectedTarget()
        {
            try
            {
                pathfindingManager.ClearPath();
                ITarget target = indexToTarget[selectedTargetIndex];
                pathfindingManager.ShowPathToTarget(target, 2f);
                Vector3 targetPosition = target.GetCoordinates();
                pathRenderer.SetTarget(targetPosition);
                arrowController.Init(targetPosition);
                if (locationPrefab != null)
                {
                    if (locationInstance == null)
                    {
                        locationInstance = GameObject.Instantiate(locationPrefab);
                    }

                    locationInstance.transform.position = targetPosition;
                    locationInstance.SetActive(true);
                }
               
                StartCoroutine(ReachTarget());
            }
            catch (KeyNotFoundException nokey)
            {
                pathfindingManager.ClearPath();
            }
        }

        /// <summary>
        /// Adds targets to the dropdown selection.
        /// </summary>
        /// <param name="allTargets">List of targets.</param>
        private void PathFindingReady(List<ITarget> allTargets)
        {
            targets = allTargets;
            targetSelectionDropDown.ClearOptions();

            List<string> allTargetNames = new List<string>();
            allTargetNames.Add("No target");
            for (int i = 0; i < allTargets.Count; i++)
            {
                string targetName = allTargets[i].GetTargetName();
                allTargetNames.Add(targetName);                
                indexToTarget.Add(i+1, allTargets[i]);
            }
            targetSelectionDropDown.AddOptions(allTargetNames);
        }

        /// <summary>
        /// Called when target selected.
        /// </summary>
        /// <param name="val">index of target selected</param>
        private void OnTargetSelected(int val)
        {
            this.selectedTargetIndex = val;
            if (val == 0)
            { 
                pathfindingManager.ClearPath();
                locationInstance.SetActive(false);
            }
        }

        /// <summary>
        /// starts checking if we reach the target.
        /// </summary>
        /// <returns></returns>
        IEnumerator ReachTarget()
        {
            yield return new WaitForFixedUpdate();
            if (locationInstance != null)
            {
                Vector3 currentPos = new Vector3(mainCameraTransform.position.x, locationInstance.transform.position.y, mainCameraTransform.position.z);
                
                if (Vector3.Distance(currentPos, locationInstance.transform.position) < 1f)
                {
                    targetReached.Invoke();
                    yield return new WaitForSeconds(3f);
                    locationInstance.gameObject.SetActive(false);
                    targetReachedHint.gameObject.SetActive(false);
                    StopAllCoroutines();
                }
                else
                {
                    StartCoroutine(ReachTarget());
                }
            }
        }
    }
}
