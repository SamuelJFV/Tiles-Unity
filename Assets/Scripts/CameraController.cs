
using UnityEngine;
using System.Collections;
 
public class CameraController : MonoBehaviour 
{
    public Transform cameraTransform;
    public Transform selectionTransform;
    public GameObject test;

    public float normalSpeed;
    public float fastSpeed;
    float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 selectedStartPosition;
    public Vector3 selectedCurrentPosition;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    HexGridLayout hexGridLayout = new HexGridLayout();

    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMovementInput();
        HandleMouseInput();
    }
    
    void HandleMouseInput()
    {
        Select();
        Drag();
        Scroll();
        Rotate();
    }
    public void HandleMovementInput()
    {
        CameraPosition();
        CameraRotation();
        CameraZoom();
    }
    public void Select()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                selectedStartPosition = hexGridLayout.SnapToGrid(ray.GetPoint(entry));
                selectionTransform.position = selectedStartPosition;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        GameObject testObj = Instantiate(test);
                        testObj.transform.position = hexGridLayout.SnapToGrid(new Vector3(i,0,j));
                    }
                }

            }
        }
    }
    public void Drag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }
    public void Scroll()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y*zoomAmount;
        }
    }
    public void Rotate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up*(-difference.x/5.0f));
        }
        
    }

    public void CameraPosition()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += transform.forward*movementSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += transform.right*movementSpeed*(-1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += transform.forward*movementSpeed*(-1);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += transform.right*movementSpeed;
        }
        
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime*movementTime);
    }
    public void CameraRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up*rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up*rotationAmount*(-1));
        }

        //newRotation *= Quaternion.Euler(Vector3.up*Input.GetAxis("Mouse X")*rotationAmount);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime*movementTime);
    }
    public void CameraZoom()
    {
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime*movementTime);
    }
}