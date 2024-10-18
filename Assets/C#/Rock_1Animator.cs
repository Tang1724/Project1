using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_1Animator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Floor floor;
    private void Awake(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        floor = GetComponent<Floor>();
    }

    private void Update(){

        SetAnimation();
    }

    private void SetAnimation(){
        anim.SetFloat("Mass", floor.totalMass);
    }

}