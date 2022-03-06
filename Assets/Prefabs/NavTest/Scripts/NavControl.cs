using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;

public class NavControl : MonoBehaviour
{
    //prefab to instantiate and show the navigation.
    public GameObject pathPrefab;

    //navigation points to target.
    public List<IOctreeCoordnateObject> allNavPoints;

    //guide bot controller.
    public GuideBotController botController;

    private static NavControl mInstance = null;

    public static NavControl Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
    }

    //Initializes navigation for nav bot
    public void InitGuideBot()
    {
        botController.StartNavigation(allNavPoints);
    }
}
