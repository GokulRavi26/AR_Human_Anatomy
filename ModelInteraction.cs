using UnityEngine;
public class ModelInteraction :MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public float zoomSpeed = 0.5f; // Speed of zoom
    public float panSpeed = 0.1f; // Speed of panning
    public float minScale = 0.5f; // Minimum zoom scale
    public float maxScale = 2.5f; // Maximum zoom scale
    private void Update()
    {
        if (Input.touchCount == 1) // Single finger for horizontal rotation
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotX = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
transform.Rotate(Vector3.up, -rotX, Space.World); // Left-right rotation only
            }
        }
        else if (Input.touchCount == 2) // Two fingers for pinch zoom and pan
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            // Calculate distance between touches
            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;
            float difference = currentDistance - prevDistance;
            // Zoom (Pinch to Scale)
            float scaleFactor = 1 + (difference * zoomSpeed * Time.deltaTime);
transform.localScale = Vector3.ClampMagnitude(transform.localScale * scaleFactor, maxScale);
transform.localScale = new Vector3(
Mathf.Clamp(transform.localScale.x, minScale, maxScale),
Mathf.Clamp(transform.localScale.y, minScale, maxScale),
Mathf.Clamp(transform.localScale.z, minScale, maxScale)
            );
            // Panning (Moving the model)
            if (touch1.phase == TouchPhase.Moved&& touch2.phase == TouchPhase.Moved)
            {
                Vector2 averageDelta = (touch1.deltaPosition + touch2.deltaPosition) / 2;
                Vector3 move = new Vector3(averageDelta.x * panSpeed * Time.deltaTime, averageDelta.y * panSpeed * Time.deltaTime, 0);
transform.position += move;
            }
        }
    }
}
