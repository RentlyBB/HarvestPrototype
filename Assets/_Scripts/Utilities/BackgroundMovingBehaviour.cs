using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovingBehaviour : MonoBehaviour {
    
   [SerializeField] private float backgroundSpeed = 2f;  // Speed of the background movement
    [SerializeField] private Transform[] backgrounds;     // Array of the three background objects
    [SerializeField] private Camera mainCamera;           // Reference to the main camera

    private float spriteWidth;                            // Width of a single background sprite

    void Start()
    {
        // Calculate the width of the background sprite assuming all are of equal size
        spriteWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        MoveBackgrounds();
        CheckAndTeleportBackground();
    }

    // Move the background sprites to the left over time
    void MoveBackgrounds()
    {
        foreach (Transform background in backgrounds)
        {
            background.position += Vector3.left * backgroundSpeed * Time.deltaTime;
        }
    }

    // Check if the background sprite is out of the camera's view and teleport it
    void CheckAndTeleportBackground()
    {
        float leftCameraEdge = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;

        foreach (Transform background in backgrounds)
        {
            if (background.position.x + spriteWidth / 2 < leftCameraEdge)
            {
                // Teleport the background to the right end of the chain
                float rightmostPosition = GetRightmostBackgroundPosition();
                background.position = new Vector3(rightmostPosition + spriteWidth, background.position.y, background.position.z);
            }
        }
    }

    // Get the x position of the rightmost background sprite
    float GetRightmostBackgroundPosition()
    {
        float rightmostPosition = backgrounds[0].position.x;
        foreach (Transform background in backgrounds)
        {
            if (background.position.x > rightmostPosition)
            {
                rightmostPosition = background.position.x;
            }
        }
        return rightmostPosition;
    }

}