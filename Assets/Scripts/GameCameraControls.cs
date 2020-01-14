using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCameraControls : MonoBehaviour
{
    public static GameCameraControls Instance;

    //Reference our PlayerController so we can keep if we're strifing
    public PlayerController playerController;

    //This will be our script that will be responsible for basic camera movement
    public float cameraRotationSpeed = 1;

    public Transform camPivotPoint, playerPoint;

    float mousePositionX, mousePositionY;

    //We'll have a camera offset so that when we are strifing, we look at what our model is looking at
    float camOffsetX, camOffsetY;
    public float CameraOffsetX
    {
        get
        {
            return camOffsetX;
        }
    }
    public float CameraOffsetY
    {
        get
        {
            return camOffsetX;
        }
    }

    //This will be for if there's a wall in the way
    public Transform obstructionTarget;
    float zoomSpeed = 2f;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        } 
        #endregion
    }

    void Update()
    {
        //Have obstruction equal to our camPivotPoint
        obstructionTarget = camPivotPoint;

        //Make sure out cursor is in the center
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerController.isLeftStrifing)
            SetCameraXOffset(50f);
        else if (playerController.isRightStrifing)
            SetCameraXOffset(-50f);
        else
            SetCameraXOffset(0f);
    }

    private void FixedUpdate()
    {
        RunMouseControls();
        ViewObstructed();
    }

    void RunMouseControls()
    {
        mousePositionX += Input.GetAxis("Mouse X") * cameraRotationSpeed * Time.deltaTime;
        mousePositionY -= Input.GetAxis("Mouse Y") * cameraRotationSpeed * Time.deltaTime;

        //Huh?
        mousePositionY = Mathf.Clamp(mousePositionY, -35, 60);

        transform.LookAt(camPivotPoint);

        camPivotPoint.rotation = Quaternion.Euler(mousePositionY + camOffsetY, mousePositionX + camOffsetX, 0);
        playerPoint.rotation = Quaternion.Euler(0, mousePositionX, 0);
    }

    void ViewObstructed()
    {
        MeshRenderer obstructionRender = obstructionTarget.gameObject.GetComponent<MeshRenderer>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, camPivotPoint.position - transform.position, out hit, 4.5f))
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                obstructionTarget = hit.transform;
                obstructionRender.shadowCastingMode = ShadowCastingMode.ShadowsOnly;

                if (Vector3.Distance(obstructionTarget.position, transform.position) >= 3f &&
                    Vector3.Distance(transform.position, camPivotPoint.position) >= 1.5f)
                {
                    transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
                }
            }
            else
            {
                obstructionRender.shadowCastingMode = ShadowCastingMode.On;
                if (Vector3.Distance(transform.position, camPivotPoint.position) < 4.5f)
                {
                    transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void SetCameraXOffset(float val)
    {
        camOffsetX = val;
    }

    public void SetCameraYOffset(float val)
    {
        camOffsetY = val;
    }
}
