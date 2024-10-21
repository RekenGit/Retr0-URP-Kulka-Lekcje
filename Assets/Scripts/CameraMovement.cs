using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField][Range(0, 1)] private float smoothnes;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private Vector3 camClosePos = new Vector3(1.5f, 0.5f, 0f);
    private float mouseMove;
    private LayerMask camUpLayer;
    private bool setCamPosUp;
    private bool SetCamPos
    {
        get { return setCamPosUp; }
        set
        {
            if (setCamPosUp != value)
            {
                setCamPosUp = value;
                StartCoroutine(SetCameraRotationAfterSec(0.1f));
                //playerCamera.transform.LookAt(player.position);
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera.transform.position = offset;
        transform.position = player.position;
        playerCamera.transform.LookAt(player.position);
        camUpLayer = LayerMask.GetMask("CamUp");
    }

    private void Update()
    {
        mouseMove += Input.GetAxis("Mouse X");
    }

    void LateUpdate()
    {
        SetCamPos = Physics.CheckSphere(player.position - new Vector3(0f, 3f, 0f), 3f, camUpLayer);
        if (setCamPosUp)
        {
            playerCamera.transform.localPosition = camClosePos;
        }
        else
        {
            playerCamera.transform.localPosition = offset;
        }

        //Vector3 smothedPosition = Vector3.Lerp(transform.position, player.position, smoothnes);
        transform.localRotation = Quaternion.Euler(0f, mouseMove, 0f);
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothnes);
    }

    private IEnumerator SetCameraRotationAfterSec(float sec)
    {
        yield return new WaitForSeconds(sec);
        playerCamera.transform.LookAt(player.position);
    }
}
