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

        float distanceToGround = 0;
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.CompareTag("Terrain")){
                distanceToGround = Vector3.Distance(transform.position, hit.point);
            }
        }

        Keyboard keyboard = Keyboard.current;
        if(keyboard.spaceKey.wasPressedThisFrame && distanceToGround < 0.5){
            newVel.y = jumpHeight;
        }

        rb.velocity = newVel;
    }
}
