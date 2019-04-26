﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Setting_3C setting3C = null;

    bool isLanded = false;
    bool isDead = false;

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            return;
        }

        Vector3 playerMeshCenter = this.transform.GetChild(0).position;
        Vector3 playerMeshBottom = this.transform.position; // anchor point is (0.5, 0.0)
        Vector3 lineCast_Endpoint = playerMeshBottom - new Vector3(0.0f, 0.1f, 0.0f);

        RaycastHit hitTarget;
        if (Physics.Linecast(playerMeshCenter, lineCast_Endpoint, out hitTarget)) {
            if (!isLanded) {
                LandedOnBlock(hitTarget.transform.GetComponent<Block>());
            }
        }
        else {
            isLanded = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                if (isLanded) {
                    this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, setting3C.jumpForce, 0.0f));
                }
            }
        }
        else if(Input.GetButtonDown("Fire1")) {
            if (isLanded) {
                this.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, setting3C.jumpForce, 0.0f));
            }
        }
    }

    void LandedOnBlock(Block block)
    {
        isLanded = true;

        if (block != null) {
            block.Hit();
        }
    }

    public void Dead(int side)
    {
        isDead = true;
        Destroy(this.GetComponent<Collider>());
        this.GetComponent<Rigidbody>().AddForce(new Vector3(side * setting3C.deadForce, 0.0f, 0.0f));
    }
}
