using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    //int speed = 100;
    public Transform Target;
    private Vector3 Offset;
    private float y;
    public float SpeedFollow = 5f;

    void Start()
    {
        Offset = transform.position;   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //float step = speed * Time.deltaTime;
        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, GameObject.Find("Player").transform.localPosition, Time.deltaTime * step);
        Vector3 followPos = Target.position + 
            Offset;
        RaycastHit hit;
        if (Physics.Raycast(Target.position, Vector3.down, out hit, 2.5f))
        {

            y = Mathf.Lerp(y, hit.point.y, Time.deltaTime * SpeedFollow);
        }
        else {

            y = Mathf.Lerp(y, Target.position.y, Time.deltaTime * SpeedFollow);

        }
        followPos.y = Offset.y + y;
        transform.position = followPos;
    }

}
