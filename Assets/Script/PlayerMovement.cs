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

    //te deja saltar hasta 0.1s despues de haber dejado el suelo.
    [SerializeField] float coyoteTime = 0.1f;
    //si presionas el salto 0.1s antes el salto se guarda y vuelve a saltar.
    [SerializeField] float jumpBuffer = 0.1f;

    //Las variables float guardan cuando ocurrio cada cosa
    float ultimoTiempoEnSuelo;
    //el -10 significa que paso hace mucho tiempo, para que al iniciar no se active nada
    float ultimoTiempoSalto = -10f;
    void Movement()
    {
        //getaxis devuelve un numero entre -1 y 1, tanto izquierda a derecha y viceversa
        float HorizontalInput = Input.GetAxis("Horizontal"); 
        float VerticalInput = Input.GetAxis("Vertical");

        bool enSuelo = characterController.isGrounded;

        if (enSuelo)
        {
            //Si esta en el suelo, anota la hora actual, alimentando el salto coyote.
            ultimoTiempoEnSuelo = Time.time;
            //si venia vatendo, lo fijara en -5 la gravedad no se acumula
            //si no, seguirias acelerando hacia abajo, al caerte en un borde saldrias disparado.
            if (velocity.y < 0) velocity.y = -5f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            //getButtonDown es verdadero solo en el frame presionado, este anotara la hora para el jump buffer.
            ultimoTiempoSalto = Time.time;
        }

        //esto es como decir ¿estuve en el suelo hace 0.1s o menos?
        bool puedeSaltar = Time.time - ultimoTiempoEnSuelo <= coyoteTime;
        //¿Presione saltar hace 0.1s o menos?
        bool quiereSaltar = Time.time - ultimoTiempoSalto <= jumpBuffer;

        if(puedeSaltar && quiereSaltar)//si los dos son verdaderos
        {
            //da el impulso inicial, el calculo SQRT es una raiz
            //es inspirado en la formula vertical de un salto
            velocity.y = Mathf.Sqrt(heightJump * -2f*gravity);
            //Luego resetea los dos tiempos a -10, para que no pueda saltar dos veces con la misma pulsacion o aterrizaje
            ultimoTiempoSalto = -10f;
            ultimoTiempoEnSuelo = -10f;
        }

        // cada frane resta velocidad vertical, el deltatime hace que sea igual en una pc rapida o lenta.
        velocity.y += gravity * Time.deltaTime;

        //combina las direcciones del jugador, esto para que cuando gires la camara, W siempre va hacia donde mires
        Vector3 move = transform.right * HorizontalInput + transform.forward * VerticalInput;

        //En total es la velocidad al caminar, sumamos la velocidad vertical (salto)
        Vector3 total = move * _speed + velocity;
        
        //desplazas el personaje esa distancia en este frame, respetando las colisiones
        characterController.Move(total * Time.deltaTime);
        //con translate, esto no se hubiera conseguido, por que atravesaria colisiones.
    }
}
