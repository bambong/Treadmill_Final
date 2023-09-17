using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float power_Y;
    [SerializeField] float power_XZ;
    bool activing;

    public void Active(bool active)
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        if (active)
        {
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
        }

        activing = active;
    }
    public void Jump()
    {
        Vector2 normalVel = new Vector2(rb.velocity.x, rb.velocity.z).normalized;
        Vector3 jumpDir = new Vector3(normalVel.x * power_XZ, power_Y, normalVel.y * power_XZ);

        rb.velocity = jumpDir;
        rb.angularVelocity = Vector3.zero;
    }
    public void SetPosition(Vector3 position)
    {
        rb.transform.position = position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) Jump();
    }
}
