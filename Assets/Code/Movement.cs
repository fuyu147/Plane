using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject mManager;

    Config mConfig;

    float mSpeed = 10;
    float mSpeedRunModifier = 1.7f;


    List<(KeyCode[], Vector3)> mMovementKeys;
    KeyCode[] mRunKeys;

    void UpdateKeys()
    {
        mMovementKeys = new()
        {
            (mConfig.movementConfig.Up.ToArray(), Vector3.up),
            (mConfig.movementConfig.Down.ToArray(), Vector3.down),
            (mConfig.movementConfig.Forward.ToArray(), Vector3.forward),
            (mConfig.movementConfig.Left.ToArray(), Vector3.left),
            (mConfig.movementConfig.Backward.ToArray(), Vector3.back),
            (mConfig.movementConfig.Right.ToArray(), Vector3.right)
        };
        mRunKeys = mConfig.movementConfig.Run.ToArray();
    }

    public void Start()
    {
        mConfig = mManager.GetComponent<Config>();
        UpdateKeys();
    }

    public void Update()
    {
        UpdateKeys();
        var speed = mSpeed;

        foreach (var key in mRunKeys)
            if (Input.GetKey(key))
                speed = mSpeed * mSpeedRunModifier;

        foreach (var (keys, direction) in mMovementKeys)
        {
            Vector3 movement = new();

            foreach (var key in keys)
                if (Input.GetKey(key))
                    movement += direction;

            movement.Normalize();
            transform.Translate(Time.deltaTime * speed * movement);
        }
    }
}
