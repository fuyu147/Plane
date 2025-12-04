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

        mMovementKeys = new()
        {
            (config.movementConfig.Up.ToArray(), Vector3.up),
            (config.movementConfig.Down.ToArray(), Vector3.down),
            (config.movementConfig.Forward.ToArray(), Vector3.forward),
            (config.movementConfig.Left.ToArray(), Vector3.left),
            (config.movementConfig.Backward.ToArray(), Vector3.back),
            (config.movementConfig.Right.ToArray(), Vector3.right)
        };
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
