using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerMoveControl playerMoveControl;
    private void Awake(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMoveControl = GetComponent<PlayerMoveControl>();
    }

    private void Update(){

        SetAnimation();
    }

    private void SetAnimation(){
        anim.SetFloat("velocityX", math.abs(rb.velocity.x));
        anim.SetFloat("velocityY", rb.velocity.y);
        anim.SetBool("isGrounded", playerMoveControl.isGrounded);
    }

}
