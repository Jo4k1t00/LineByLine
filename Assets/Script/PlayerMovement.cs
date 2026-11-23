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


    [SerializeField] float coyoteTime = 0.1f;
    [SerializeField] float jumpBuffer = 0.1f;

    float ultimoTiempoEnSuelo;
    float ultimoTiempoSalto = -10f;
    void Movement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal"); 
        float VerticalInput = Input.GetAxis("Vertical");

        bool enSuelo = characterController.isGrounded;

        if (enSuelo)
        {
            ultimoTiempoEnSuelo = Time.time;
            if (velocity.y < 0) velocity.y = -5f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            ultimoTiempoSalto = Time.time;
        }

        bool puedeSaltar = Time.time - ultimoTiempoEnSuelo <= coyoteTime;
        bool quiereSaltar = Time.time - ultimoTiempoSalto <= jumpBuffer;

        if(puedeSaltar && quiereSaltar)
        {
            velocity.y = Mathf.Sqrt(heightJump * -2f*gravity);
            ultimoTiempoSalto = -10f;
            ultimoTiempoEnSuelo = -10f;
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 move = transform.right * HorizontalInput + transform.forward * VerticalInput;
        Vector3 total = move * _speed + velocity;
        
        characterController.Move(total * Time.deltaTime);
    }
}
