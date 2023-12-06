using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public Transform personalCamera;

    public override void OnNetworkSpawn()
    {
        CinemachineVirtualCamera vcam = personalCamera.gameObject.GetComponent<CinemachineVirtualCamera>();
        if (IsOwner)
        {
            vcam.Priority = 1;
        }
        else
        {
            vcam.Priority = 0;
        }
    }


    [Header("Movement")]
    public float playerSpeed;
    public float jumpHeight;
    public float gravity;
    [SerializeField] float velocity;

    [Header("Lanes")]
    [SerializeField] float laneDistance = 4; //distancia entre duas lanes
    private int desiredLane = 1; //0:esquerda 1:meio 2:direita
    private float desiredHeight = 1;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeLane();
    }

    void ChangeLane()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            desiredLane = Mathf.Max(desiredLane - 1, -1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            desiredLane = Mathf.Min(desiredLane + 1, 3);
        }
        if (Input.GetKey(KeyCode.UpArrow) && (desiredLane == -1 || desiredLane == 3))
        {
            desiredHeight = Mathf.Max(desiredHeight + 1, 5);
        }
        if (Input.GetKey(KeyCode.DownArrow) && (desiredLane == -1 || desiredLane == 3))
        {
            desiredHeight = Mathf.Min(desiredHeight - 1, -2);
        }

        desiredHeight = Mathf.Clamp(desiredHeight, 1f, 9f);
        if (desiredLane >= 0 && desiredLane <= 2)
        {
            desiredHeight = 1f;
            Vector3 targetPosition = new Vector3((desiredLane - 1) * laneDistance, 0, transform.position.z + (playerSpeed * velocity));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);

        }
        else if (desiredLane == -1)
        {
            Vector3 targetPosition = new Vector3(-1.70f * laneDistance, desiredHeight, transform.position.z + (playerSpeed * velocity));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
        }
        else if (desiredLane == 3)
        {
            Vector3 targetPosition = new Vector3(1.70f * laneDistance, desiredHeight, transform.position.z + (playerSpeed * velocity));
            transform.position = Vector3.Lerp(transform.position, targetPosition, 10 * Time.deltaTime);
        }
    }
}

