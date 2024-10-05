using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerState playerState;
    private PlayerMoveControl playerMoveControl;
    private void Awake(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        playerMoveControl = GetComponent<PlayerMoveControl>();
    }

    private void Update(){

        SetAnimation();
    }

    private void SetAnimation(){
        anim.SetFloat("Full", playerState.CurrentMass);
         anim.SetFloat("Fly", playerState.CurrentMass);
        anim.SetBool("death", playerMoveControl.death);
        anim.SetBool("Door1", playerMoveControl.Door1);
    }

}
