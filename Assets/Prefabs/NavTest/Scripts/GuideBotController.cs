using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuideBotController : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;
    public Animator anim;
    public List<Transform> pointList;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //anim.Play("Idle");
        StartCoroutine("Moving");
        anim.Play("Moving");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveToTarget(Transform target)
    {
        
       
    }
    IEnumerator Moving( )
    {  
        
        
        counter = 0;
        while (transform.position != pointList[pointList.Count-1].position)
        {
            //rotate towards target.
            
            Vector3 relativePos = pointList[counter].position - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
            //move towards target.
            transform.position = Vector3.MoveTowards(transform.position, pointList[counter].position, moveSpeed * Time.deltaTime);
            Debug.Log("NotReached");
            if (Vector3.Distance(transform.position, pointList[counter].position) <= 0)
            {
                Debug.Log("Target Reached");
                counter++;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                break;
            }
            yield return null;

        }

        Debug.Log("before Something");
        yield return null;
        Debug.Log("something");
    }
}

