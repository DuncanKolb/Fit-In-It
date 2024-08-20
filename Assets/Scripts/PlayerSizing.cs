using UnityEngine;

public class PlayerSizing : MonoBehaviour
{
    public float scaleFactor = 0.01f; // Sensitivity for scaling
    private Vector3 initialScale;
    private float screenWidth;
    private float screenHeight;


    void Start()
    {
        // Store the initial scale of the GameObject
        initialScale = transform.localScale;

        // Get the screen width and height, and calculate the center
        screenWidth = Screen.width / 2.0f;
        screenHeight = Screen.height / 2.0f;
    }

    void Update()
    {
        // Get the mouse position relative to the screen center

        

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float distanceFromCenterX = mouseX - screenWidth;
        float distanceFromCenterY = mouseY - screenHeight;
        // Calculate the new scale based on the distance from the screen center
        float newScaleX = initialScale.x + distanceFromCenterX * scaleFactor;
        float newScaleY = initialScale.y + distanceFromCenterY * scaleFactor;

        // Apply the new scale to the GameObject
        transform.localScale = new Vector3(newScaleX/2, newScaleY/2, initialScale.z);
        transform.position = new Vector3(newScaleX / 4, newScaleY / 4, transform.position.z);
    }
}
