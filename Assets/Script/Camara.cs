using UnityEngine;
using UnityEngine.UI;
public class Camara : MonoBehaviour
{
    public Transform Player;

    [SerializeField]
    private float _sensibilidad = 3f;
    private float _rotacionVertical = 0;
    private float _rotacionHorizontal = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        float mouseX = Input.GetAxis("Mouse X") * _sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y")*_sensibilidad;
        _rotacionVertical -= mouseY;

        _rotacionVertical = Mathf.Clamp(_rotacionVertical,-90,90);

        transform.localRotation = Quaternion.Euler(_rotacionVertical,0,0);

        Player.Rotate(Vector3.up * mouseX);

        //Debug.Log($" MouseEnX={mouseX:F1} MouseEnY={mouseY:F1}");
    }
}
