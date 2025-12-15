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
    [SerializeField] float mJumpImpulse = 120f; // jumps about 1 meter assuming 50 kg mass


    List<(KeyCode[], Vector3)> mMovementKeys;
    KeyCode[] mRunKeys;
    KeyCode[] mJumpKeys;

    void UpdateConfig()
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
        UpdateConfig();
    }

    public void FixedUpdate()
    {
        var _speed = mSpeed;

        foreach (var key in mRunKeys)
        {
            if (Input.GetKey(key))
            {
                _speed = mSpeed * mSpeedRunModifier;
                break;
            }
        }

        Vector3 _movement = Vector3.zero;
        foreach (var (keys, direction) in mMovementKeys)
            foreach (var key in keys)
                if (Input.GetKey(key)) _movement += direction;

        _movement.Normalize();

        var _velocity = _speed
                        * Time.fixedDeltaTime
                        * transform.TransformDirection(_movement);

        mRigidbody.MovePosition(
                mRigidbody.position +
                _velocity);

        if (Manager.DEBUG && false) print($"Movement.cs :: velocity: {_velocity}");

        bool _jumped = false;
        foreach (var key in mJumpKeys) if (Input.GetKey(key)) _jumped = true;

        var _isGrounded = IsGrounded();

        if (Manager.DEBUG && true)
            print($"Movement.cs :: {_jumped} {_isGrounded}");

        var _jumpVelocity = Vector3.up * mJumpImpulse;
        if (Manager.DEBUG && true)
            print($"Movement.cs :: jump velocity {_jumpVelocity}");

        if (_jumped && _isGrounded)
            mRigidbody.AddForce(_jumpVelocity, ForceMode.Impulse);
    }

    void Update()
    {
        UpdateConfig();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
                mCollider.bounds.center,
                Vector3.down,
                mCollider.bounds.extents.y + 0.1f);
    }
}
