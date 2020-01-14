using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCameraControls : MonoBehaviour
{
    //This will be our script that will be responsible for basic camera movement
    public float cameraRotationSpeed = 1;

    public Transform camPivotPoint, playerPoint;

    float mousePositionX, mousePositionY;

    //This will be for if there's a wall in the way
    public Transform obstructionTarget;
    float zoomSpeed = 2f;

    void Update()
    {
        //Have obstruction equal to our camPivotPoint
        obstructionTarget = camPivotPoint;

        //Make sure out cursor is in the center
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        camPivotPoint.rotation = Quaternion.Euler(mousePositionY, mousePositionX, 0);
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
}
