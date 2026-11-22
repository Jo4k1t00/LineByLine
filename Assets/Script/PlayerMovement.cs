using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float heightJump = 1.5f;
    [SerializeField]
    private float gravity = -20f;
    Vector3 velocity;
    private CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    void Movement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal"); 
        float VerticalInput = Input.GetAxis("Vertical");

        if(characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * HorizontalInput + transform.forward * VerticalInput;
        characterController.Move(move * _speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(heightJump * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
