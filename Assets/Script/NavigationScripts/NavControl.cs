using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;
using UnityEngine.Events;
using TMPro;

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
    //Event triggered when target reached.
    public UnityEvent FinishedNavigation = new UnityEvent();
    //compass arrow controller component.
    public ArrowController arrowController;
    private static NavControl mInstance = null;

    public static NavControl Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
    }

    private void Start()
    {
        if (FinishedNavigation == null)
        {
            FinishedNavigation = new UnityEvent();
        }
        FinishedNavigation.AddListener(ShowFinishedPanel);
    }

    /// <summary>
    /// //Initializes navigation for navigation bot
    /// </summary>
    public void InitBot()
    {
        worldSpacePanel.gameObject.SetActive(true);
        robot.gameObject.SetActive(true);
        Vector3 cameraPosition = Camera.main.transform.position;
        robot.transform.position = cameraPosition + Camera.main.transform.forward * 1.1f;
        Vector3 targetPostition = new Vector3(cameraPosition.x, transform.position.y, cameraPosition.z) - transform.position;
        robot.transform.LookAt(targetPostition);
    }
    
    /// <summary>
    /// Starts navigation on GuideBotController component.
    /// </summary>
    public void StartMoving()
    {
        robot.GetComponent<GuideBotController>().StartNavigation(allNavPoints);
    }

    // Start is called before the first frame update
    private void ShowFinishedPanel()
    {
        //enable panel with default message where would you like to go.
        worldSpacePanel.SetActive(true);
        navPanelMessage.text = reachedMessage;
    }

}
