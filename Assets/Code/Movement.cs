using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] GameObject mManager;

    Config mConfig;
    Rigidbody mRigidbody;
    Collider mCollider;

    [SerializeField] float mSpeed = 10;
    [SerializeField] float mSpeedRunModifier = 1.7f;
    [SerializeField] float mJumpForce = 2000f; // assuming 50 kg mass


    List<(KeyCode[], Vector3)> mMovementKeys;
    KeyCode[] mRunKeys;
    KeyCode[] mJumpKeys;

    void UpdateKeys()
    {
        mMovementKeys = new()
        {
            (mConfig.movementConfig.Forward.ToArray(), Vector3.forward),
            (mConfig.movementConfig.Left.ToArray(), Vector3.left),
            (mConfig.movementConfig.Backward.ToArray(), Vector3.back),
            (mConfig.movementConfig.Right.ToArray(), Vector3.right)
        };

        mRunKeys = mConfig.movementConfig.Run.ToArray();
        mJumpKeys = mConfig.movementConfig.Jump.ToArray();
    }

    public void Start()
    {
        mConfig = mManager.GetComponent<Config>();
        mRigidbody = GetComponent<Rigidbody>();
        mCollider = GetComponent<Collider>();
        UpdateKeys();
    }

    public void FixedUpdate()
    {
        UpdateKeys();
        var speed = mSpeed;

        foreach (var key in mRunKeys)
        {
            if (Input.GetKey(key))
            {
                speed = mSpeed * mSpeedRunModifier;
                break;
            }
        }

        Vector3 movement = Vector3.zero;
        foreach (var (keys, direction) in mMovementKeys)
            foreach (var key in keys)
                if (Input.GetKey(key)) movement += direction;

        movement.Normalize();

        mRigidbody.MovePosition(
                mRigidbody.position +
                speed *
                Time.fixedDeltaTime *
                transform.TransformDirection(movement));

        bool jumped = false;
        foreach (var key in mJumpKeys) if (Input.GetKey(key)) jumped = true;
        var isGrounded = IsGrounded();
        if (Manager.DEBUG) print($"Movement.cs :: {jumped} {isGrounded}");
        if (jumped && isGrounded) mRigidbody.AddForce(new(0, mJumpForce, 0));
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
                mCollider.bounds.center,
                Vector3.down,
                mCollider.bounds.extents.y + 0.1f);
    }
}
