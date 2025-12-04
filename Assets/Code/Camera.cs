using UnityEngine;

public class CCamera : MonoBehaviour
{
    [SerializeField] GameObject mFollowedObject;
    [SerializeField] Vector3 mOffset;
    [SerializeField] GameObject mManager;

    CameraConfig mCameraConfig;
    float mPitch = 0.0f;
    float mYaw = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var config = mManager.GetComponent<Config>().config;

        mCameraConfig = config.cameraConfig;

        mYaw = mFollowedObject.transform.eulerAngles.y;
        mPitch = mFollowedObject.transform.eulerAngles.x;
    }

    void HandlePosition()
    {
        Quaternion rotation = Quaternion.Euler(mPitch, mYaw, 0);

        Vector3 _desiredPos = mFollowedObject.transform.position + rotation * mOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            _desiredPos,
            mCameraConfig.Speed * Time.deltaTime
        );
    }

    void HandleCameraRotation()
    {
        transform.rotation = Quaternion.Euler(mPitch, mYaw, 0);
    }

    void HandlePlayerRotation()
    {
        // Mouse input
        float _mouseX = Input.GetAxis("Mouse X") * mCameraConfig.Sensitivity * 100 * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * mCameraConfig.Sensitivity * 100 * Time.deltaTime;

        mYaw += _mouseX;
        mPitch -= _mouseY;
        mPitch = Mathf.Clamp(mPitch, -80f, 80f);

        mFollowedObject.transform.rotation = Quaternion.Euler(0, mYaw, 0);
    }

    void Update()
    {
        Camera.main.fieldOfView = mCameraConfig.FOV;
        print($"Cam fov : {mCameraConfig.FOV}");

        if (!mFollowedObject) return;

        HandlePosition();
        HandlePlayerRotation();
        HandleCameraRotation();
    }
}
