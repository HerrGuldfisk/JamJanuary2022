using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [SerializeField] float jumpHeight = 5f;
    Vector3 newVel = Vector3.zero;
    Rigidbody rb;
    bool grounded = true;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        newVel = rb.velocity;

        Keyboard keyboard = Keyboard.current;
        if(keyboard.spaceKey.IsActuated() && grounded){
            newVel.y = jumpHeight;
        }

        rb.velocity = newVel;
    }
    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Terrain"){
            grounded=true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.transform.tag == "Terrain"){
            grounded=false;
        }
    }
}
