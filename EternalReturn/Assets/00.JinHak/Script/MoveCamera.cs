using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private const float EDGE_THRESHOLD_RATE = 0.02f;
    private const float MAP_BOUNDARY_POS = 250f;

    public PlayerBase player = default;
    private bool isRooted;
    [SerializeField] private float cameraSpeed;
    private Vector3 cameraMoveVec;
    private float screenWidth;
    private float screenHeight;

    [SerializeField] private UnityEngine.UI.Image cameraRootedUI;

    private void Awake()
    {
        isRooted = true;
        UpdateCameraRootedUI(isRooted);
        SetCameraSpeed(50f);
        SetScreenResolution(Screen.width, Screen.height);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            isRooted = !isRooted;
            UpdateCameraRootedUI(isRooted);
        }

        if (isRooted)
        {
            if (player != default)
            {
                transform.position = player.transform.position;
            }
        }
        else
        {
            SetScreenResolution(Screen.width, Screen.height);

            if (Input.GetKey(KeyCode.Space) && player != default)
            {
                transform.position = player.transform.position;
                return;
            }

            cameraMoveVec = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= screenHeight * (1 - EDGE_THRESHOLD_RATE))
            {
                cameraMoveVec += transform.forward;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= screenHeight * EDGE_THRESHOLD_RATE)
            {
                cameraMoveVec += -transform.forward;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= screenWidth * EDGE_THRESHOLD_RATE)
            {
                cameraMoveVec += -transform.right;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= screenWidth * (1 - EDGE_THRESHOLD_RATE))
            {
                cameraMoveVec += transform.right;
            }

            transform.position += cameraMoveVec * cameraSpeed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -MAP_BOUNDARY_POS, MAP_BOUNDARY_POS),
                transform.position.y,
                Mathf.Clamp(transform.position.z, -MAP_BOUNDARY_POS, MAP_BOUNDARY_POS));
        }
    }

    public void SetCameraSpeed(float speed)
    {
        cameraSpeed = speed;
    }

    private void SetScreenResolution(float width, float height)
    {
        screenWidth = width;
        screenHeight = height;
    }

    private void UpdateCameraRootedUI(bool isRooted)
    {
        if (cameraRootedUI == null)
            return;

        cameraRootedUI.sprite = isRooted ? UIResources.CameraRootingOffSprite : UIResources.CameraRootingOnSprite;
    }
}
