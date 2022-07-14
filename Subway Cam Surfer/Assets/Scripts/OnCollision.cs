using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{

    public PlayerMovement player_movement;
    public Animator anim;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Obstacle")
        {
            anim.Play("fallOver");
            //player_movement.enabled = false;

        }

    }
}
