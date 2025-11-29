using UnityEngine;

public class CCamera : MonoBehaviour
{
    [SerializeField] GameObject mFollowedObject;
    [SerializeField] Vector3 mOffset;
    [SerializeField] float mCamSpeed;
    [SerializeField] float mCamSensitivity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void HandlePosition()
    {
        float yRotation = mFollowedObject.transform.eulerAngles.y;
        Quaternion yawRot = Quaternion.Euler(0f, yRotation, 0f);

        Vector3 rotatedOffset = yawRot * mOffset;

        Vector3 desiredPos = mFollowedObject.transform.position + rotatedOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            mCamSpeed * Time.deltaTime
        );
    }

    void HandleCameraRotation()
    {
        transform.LookAt(mFollowedObject.transform);
        float yRotation = mFollowedObject.transform.eulerAngles.y;
        Quaternion camRot = Quaternion.Euler(0, yRotation, 0f);
        transform.rotation = camRot;
    }

    void HandlePlayerRotation()
    {
        // Mouse input
        float _mouseX = Input.GetAxis("Mouse X") * mCamSensitivity * 100 * Time.deltaTime;
        float _mouseY = Input.GetAxis("Mouse Y") * mCamSensitivity * 100 * Time.deltaTime;

        // print($"mouse : <{_mouseX}> <{_mouseY}>");

        mFollowedObject.transform.Rotate(Vector3.up * _mouseX);
    }

    void Update()
    {
        if (!mFollowedObject) return;

        HandlePosition();
        HandlePlayerRotation();
        HandleCameraRotation();
    }
}
