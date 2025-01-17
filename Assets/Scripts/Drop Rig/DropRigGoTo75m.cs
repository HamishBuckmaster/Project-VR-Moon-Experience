﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;
using System;

//[RequireComponent(typeof(Interactable))]
// Created by Wayland Bishop for The Moon VR 3.0 project
public class DropRigGoTo75m : MonoBehaviour
{
    Animator anim;
    AudioSource sound;
    AnimatorStateInfo animationState;
    GameObject planetSettings;
    GameObject DropRig;
    Text[] text;
    bool isInterping = false;
    float aniLocation = 0;
    void Start()
    {
        DropRig = GameObject.Find("DropRig"); // Get the drop rig
        if (DropRig != null)
        {
            anim = DropRig.GetComponentInParent<Animator>(); // Get animation controller from the object
            sound = DropRig.GetComponent<AudioSource>(); // Get the sound source from the correct place in the object
            AnimatorStateInfo animationState = anim.GetCurrentAnimatorStateInfo(0); // Used Get the current animation playtime
            planetSettings = GameObject.Find("PlanetSettings"); // Get the planet settings
            sound.loop = true;
            text = DropRig.GetComponentsInChildren<Text>(); // Get all the text elements in the drop rig
        }
    }

    private void Update()
    {
        if (DropRig != null)
        {

            if (anim.GetBool("heightHasPlayed"))
            {
                text[2].text = "The current drop is " + Math.Truncate(anim.GetFloat("wingHeight")) + " Meters"; // Set the drop rig LCD text
                aniLocation = 0.75f;
            }
            else
            {
                animationState = anim.GetCurrentAnimatorStateInfo(0);
                aniLocation = animationState.normalizedTime % 1;
            }
            if (isInterping)
            {
                animationState = anim.GetCurrentAnimatorStateInfo(0);
                aniLocation = animationState.normalizedTime % 1;
                float playAmount = Mathf.Lerp(aniLocation, 0.75f, 2f * Time.deltaTime);

                anim.Play("DropRigHeight", 0, playAmount);
                StartCoroutine(Wait());
            }
        }
    }
    IEnumerator Wait()
    {

        yield return new WaitForSeconds(3);
        isInterping = false;

    }

    //Called every Update() while a Hand is hovering over this object
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        if (startingGrabType != GrabTypes.None)
        {
            anim.SetBool("heightHasPlayed", true);
            isInterping = true;
            //anim.Play("DropRigHeight", 0, 0.75f); // Play the animation
            text[3].text = ""; // Clear the instructions

        }

    }

    public void setHeight()
    {
        anim.SetBool("heightHasPlayed", true);
        isInterping = true;
        //anim.Play("DropRigHeight", 0, 0.75f); // Play the animation
        text[3].text = ""; // Clear the instructions
    }

}