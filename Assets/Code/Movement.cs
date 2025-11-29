using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject mManager;
    public float mSpeed = 10;

    List<(KeyCode[], Vector3)> mMovementKeys;

    public void Start()
    {
        var config = mManager.GetComponent<Config>().config;

        mMovementKeys = new() { };
        mMovementKeys.Add((config.mMovementUp.ToArray(), Vector3.up));
        mMovementKeys.Add((config.mMovementDown.ToArray(), Vector3.down));
        mMovementKeys.Add((config.mMovementForward.ToArray(), Vector3.forward));
        mMovementKeys.Add((config.mMovementLeft.ToArray(), Vector3.left));
        mMovementKeys.Add((config.mMovementBackward.ToArray(), Vector3.back));
        mMovementKeys.Add((config.mMovementRight.ToArray(), Vector3.right));
    }

    public void Update()
    {
        foreach (var (keys, direction) in mMovementKeys)
        {
            foreach (var key in keys)
                if (Input.GetKey(key))
                    transform.Translate(Time.deltaTime * mSpeed * direction);
        }
    }
}
