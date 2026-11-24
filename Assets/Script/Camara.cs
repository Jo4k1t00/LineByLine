using UnityEngine;
using UnityEngine.UI;
public class Camara : MonoBehaviour
{
    //Referencia al objeto jugador, la camra es hija del jugador pero necesita girarlo a el.
    public Transform Player;

    //que tan rapido va el mouse
    [SerializeField]
    private float _sensibilidad = 3f;
    //guarda el angulo acumulado de arriba/abajo
    private float _rotacionVertical = 0;

    // el cursor queda fijo al centro de la pantalla sin chocar con el borde y ocultar con el borde.
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        FirstPerson();

    }
    void FirstPerson()
    {
        //Mouse x e y devuelven cuanto se movio el mouse en este frame, se multiplica por la sensibilidad si mas rapido
        float mouseX = Input.GetAxis("Mouse X") * _sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y")*_sensibilidad;

        //Se resta porque en Unity, un angulo X positivo inclina la camara hacia abajo.
        //si no restaras tendras la camara invertida.
        _rotacionVertical -= mouseY;

        //limitas el angulo. Sin esto podrias dar la vuelta completa
        _rotacionVertical = Mathf.Clamp(_rotacionVertical,-90,90);

        //rotacion relativa al padre, importante por que el jugador ya va girando a los lados
        transform.localRotation = Quaternion.Euler(_rotacionVertical,0,0);

        //Aqui no giras la camara, giaras al jugador completo sobre el eje vertical
        Player.Rotate(Vector3.up * mouseX);

        //Debug.Log($" MouseEnX={mouseX:F1} MouseEnY={mouseY:F1}");
    }
}
