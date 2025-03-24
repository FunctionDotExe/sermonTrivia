using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Bob Parameters")]
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    
    private float defaultPosY = 0;
    private float timer = 0;
    
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }
    
    void Update()
    {
        // Get right stick input for movement
        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");
        
        // Check if player is moving (including keyboard and controller input)
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f ||
                       Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
                       Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        
        if (isMoving)
        {
            // Increment the timer and calculate new position
            timer += Time.deltaTime * walkingBobbingSpeed;
            float newY = defaultPosY + Mathf.Sin(timer) * bobbingAmount;
            
            // Apply the new position
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            // Reset to default position when not moving
            timer = 0;
            float newY = Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed);
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }
} 