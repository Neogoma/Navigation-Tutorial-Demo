using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Neogoma.Octree;
using TMPro;
public class GuideBotController : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public Animator anim;
    public List<Transform> pointList;
    public TMP_Text msg;
    public GameObject panel;
    List<IOctreeCoordnateObject> allNavPoints;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim.Play("Idle");
    }

    public void StartNavigation(List<IOctreeCoordnateObject> m_allNavPoints)
    {
        transform.position = Camera.main.transform.position;
        transform.position += new Vector3(0, 0, -10f);
        allNavPoints = m_allNavPoints;
        foreach(IOctreeCoordnateObject point in allNavPoints)
        {
            Debug.Log(point.GetCoordnates());
        }
        StartCoroutine("Moving");
        anim.Play("Moving");
    }

    IEnumerator Moving()
    {  
        counter = 0;
        while (transform.position != allNavPoints[allNavPoints.Count-1].GetCoordnates())
        {
            //rotate towards target.
            
            Vector3 relativePos = allNavPoints[counter].GetCoordnates() - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
            //move towards target.
            transform.position = Vector3.MoveTowards(transform.position, allNavPoints[counter].GetCoordnates(), moveSpeed * Time.deltaTime);
            Debug.Log("NotReached");
            if (Vector3.Distance(transform.position, allNavPoints[counter].GetCoordnates()) <= 0)
            {
                Debug.Log("Target Reached");
                relativePos = Camera.main.transform.position - transform.position;
                toRotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);


                msg.text = "Reached Destination";
                panel.gameObject.SetActive(true);
                counter++;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                break;
            }
            yield return null;

        }

        yield return null;
    }
}

