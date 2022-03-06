using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.HoboDream.Impl;
using com.Neogoma.Octree;
using com.Neogoma.Stardust.Navigation;

public sealed class NavControl : MonoBehaviour
{
    public GameObject pathInstance;

    public List<IOctreeCoordnateObject> allNavPoints;

    private static NavControl mInstance = null;
    public static NavControl Instance
    {
        get { return mInstance; }
    }

    void Awake()
    {
        mInstance = this;
    }

}
