using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Camera
    public Camera cameraObject;
    public float moveSpeed = 10f;

    // Hex map
    public HexMap hexMap;

    // Camera bounds
    float yOffset = 0;
    float maxYPos = 0;
    float minYPos = 0;


    // Zoom camera
    public void ZoomCamera()
    {

    }


    // Calculate camera bounds
    public void CalculateCameraBounds()
    {
        int hexHeight = 112;
        int mapHeight = hexHeight * (hexMap.mapRadius * 2 + 1);
        int heightDiff = mapHeight - Screen.height;
        Debug.Log("map height: " + mapHeight);
        Debug.Log("screen height: " + Screen.height);
        if (heightDiff > 0)
        {
            yOffset = (float)heightDiff / 100 / 2;
            maxYPos = yOffset;
            minYPos = -yOffset;
            Debug.Log("y offset: " + yOffset);
        }
    }


    // Move camera
    public void MoveCamera()
    {
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        if (moveY != 0)
        {
            cameraObject.transform.position = new Vector3(0, Mathf.Clamp(transform.position.y + moveY, minYPos, maxYPos), -10);
        }
    }


    // Calculate orthographic size
    public void CalculateOrthographicSize()
    {
        float orthoSize = (float)Screen.height / 200;
        cameraObject.orthographicSize = orthoSize;
    }


    // Start is called before the first frame update
    void Start()
    {
        CalculateCameraBounds();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateOrthographicSize();
        MoveCamera();
    }
}
