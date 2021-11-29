using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private CreateGrid grid;
    private IndividualTileManager iTM;
    private CharacterMovement characterManager;
    //private CharacterMovement chaMov;
    public GameObject assignedCamera;

    //set start position
    //temp, can later be manually selected
    [HideInInspector] public int chosenXStart = 0;
    [HideInInspector] public int chosenZStart = 0;

    [HideInInspector] public int newX = 0;
    [HideInInspector] public int newZ = 0;

    [HideInInspector] public Vector2 currentGridCords;

    [HideInInspector] public bool movedSpace = false;

    Vector3 originCameraLocation;
    Vector3 newCameraLocation;

    public bool cameraAssigned = false;
    public bool cameraIsMoving = false;
    public bool freezeMovement = false;
    void Awake()
    {
        newX = chosenXStart;
        newZ = chosenZStart;
        currentGridCords = new Vector2(newX, newZ);
    }
    void Start()
    {
        grid = this.GetComponent<CreateGrid>();
        characterManager = this.GetComponent<CharacterMovement>();
    }

    public void Update()
    {
        if (!cameraAssigned && characterManager.playerPlaced)
        {
            newCameraLocation = characterManager.newLocationVector;
            assignedCamera.transform.position = new Vector3(newCameraLocation.x,
            newCameraLocation.y + 50f, newCameraLocation.z);
            cameraAssigned = true;
        }
        if (cameraIsMoving)
        {
            moveCameraToPlayer();
        }
    }

    public void movingCamera()
    {
        cameraIsMoving = true;
        originCameraLocation = assignedCamera.transform.position;
        newCameraLocation = characterManager.newLocationVector;
    }
    public void moveCameraToPlayer()
    {
        assignedCamera.transform.position = Vector3.MoveTowards(assignedCamera.transform.position,
        new Vector3(newCameraLocation.x, newCameraLocation.y + 50f, newCameraLocation.z - 10f),
        Time.deltaTime * 50f);
        float distToPlayer = Vector3.Distance(assignedCamera.transform.position,
            new Vector3(newCameraLocation.x, newCameraLocation.y + 50f, newCameraLocation.z - 10f));
        if (distToPlayer < 1f)
        {
            cameraIsMoving = false;
        }
    }
}
