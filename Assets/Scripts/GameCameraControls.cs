using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCameraControls : MonoBehaviour
{
    public static GameCameraControls Instance;

    //Reference our camera so we can make the transition between armed and unarmed
    public Camera m_camera;

    //Armed and Unarmed Position for camera
    public Transform armedPosition;
    public Transform unarmedPosition;

    //Reference our PlayerPawn so we can check if they are dead
    public PlayerPawn player;

    //This will be our script that will be responsible for basic camera movement
    public float cameraRotationSpeed = 1;

    //The Inital Spot before spawning the player
    public Transform initialCameraPosition;

    public Transform camPivotPoint, playerPoint;

    float mousePositionX, mousePositionY;

    //We'll have a camera offset so that when we are strifing, we look at what our model is looking at
    public float camOffsetX, camOffsetY, camOffsetZ;
    public float camRotOffsetX, camRotOffsetY;

    //This will be for if there's a wall in the way
    public Transform obstructionTarget;
    float zoomSpeed = 2f;

    bool playerIsArmed = false;

    private float rotation;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            m_camera = FindObjectOfType<Camera>();
            player = GetComponentInParent<PlayerPawn>();
            camPivotPoint = gameObject.transform;
            obstructionTarget = camPivotPoint;
            ReturnToPlayerOrbit();
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
        initialCameraPosition = gameObject.transform;
    }

    private void Start()
    {
        ReturnToPlayerOrbit();
    }

    void Update()
    {
        /*Make sure the cursor is in the center of the screen
         when we are not paused!*/
        if (!GameManager.IsGamePaused && GameManager.IsGameInitialized)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //Have obstruction equal to our camPivotPoint
        obstructionTarget = camPivotPoint;

        //We create a plane that is using the world's z-coordinates
        Plane plane = new Plane(Vector3.up, transform.parent.position);

        //We take the position of the mouse to our ray point into the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



    }

    private void FixedUpdate()
    {
        if (GameManager.IsGameInitialized)
            if (!player.IsDead() && !GameManager.IsGamePaused)
            {
                RunMouseControls();
                if (playerIsArmed == false && obstructionTarget != null) ViewObstructed();
            }
            else
                CircleAroundDeathSpot();
    }

    //This is the main mouse controls
    void RunMouseControls()
    {
        //Both x and y position take the Axis of Mouse X and Y
        //which is a value between -1 and 1
        //multiply by the rotation speed
        //over a period amount of time.
        mousePositionX += Input.GetAxis("Mouse X") * cameraRotationSpeed * Time.deltaTime;
        mousePositionY -= Input.GetAxis("Mouse Y") * cameraRotationSpeed * Time.deltaTime;

        //We make sure that mouse position y is within -90 and 60
        mousePositionY = Mathf.Clamp(mousePositionY, -35, 30);

        //We want to look at the target object
        //or in this cause, have the camera point to
        //a point for it to pivot around
        transform.LookAt(camPivotPoint);

        //Our camera and player will rotate using Euler rotations
        camPivotPoint.rotation = Quaternion.Euler(mousePositionY + camRotOffsetY, mousePositionX + camRotOffsetX, 0);
        playerPoint.rotation = Quaternion.Euler(0, mousePositionX, 0);
    }

    //If the camera happens to be behind a while, it will obstruct that wall
    //allowing us to see the player.
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

    public void SetCameraXRotateOffset(float val)
    {
        camOffsetX = val;
    }

    public void GoToArmedPosition()
    {
        SetCameraXRotateOffset(-10);
        m_camera.transform.position = armedPosition.position;
        m_camera.transform.rotation = armedPosition.rotation;
        playerIsArmed = true;
    }

    public void GoToUnArmedPosition()
    {
        SetCameraXRotateOffset(0);
        m_camera.transform.position = unarmedPosition.position;
        m_camera.transform.rotation = unarmedPosition.rotation;
        playerIsArmed = false;
    }


    public void CircleAroundDeathSpot()
    {
        //Make sure that it has no parents
        if (m_camera.transform.parent != null) m_camera.transform.SetParent(null);

        camPivotPoint = GameObject.FindGameObjectWithTag("DeathSpot").transform;

        //We want to look at the target object
        //or in this cause, have the camera point to
        //a point for it to pivot around
        transform.LookAt(camPivotPoint);

        rotation += Time.deltaTime;

        camPivotPoint.rotation = Quaternion.Euler(0f, rotation, 0);
    }

    public void ReturnToPlayerOrbit()
    {
        player = GetComponentInParent<PlayerPawn>();
        m_camera.transform.SetParent(gameObject.transform);
        GoToUnArmedPosition();
        ViewObstructed();
    }

    /// <summary>
    /// Reset Camera to how it was before;
    /// </summary>
    public static void ReturnToInitialPosition()
    {
        Instance.m_camera.transform.SetParent(null);
        Instance.m_camera.transform.position = Instance.initialCameraPosition.transform.position;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
