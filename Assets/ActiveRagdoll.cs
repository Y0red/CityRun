using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    public Animator animator;
    public Rigidbody[] rigidbodies;
    public bool ragdollOn;

    void Start()
    {
        // disable ragdoll at start
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdoll(false);
    }

    void Update()
    {
        // you can use a keyboard input or any other condition to toggle the ragdoll on or off
        if (Input.GetKeyDown(KeyCode.R))
        {
            ragdollOn = !ragdollOn;
            SetRagdoll(ragdollOn);
        }
    }

    void SetRagdoll(bool value)
    {
        animator.enabled = !value;

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !value;
        }
    }
}