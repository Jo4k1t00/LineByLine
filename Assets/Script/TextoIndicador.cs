using UnityEngine;
using UnityEngine.UI;

public class TeclaIndicador : MonoBehaviour
{
    //dibuja en pantalla un indicador visual de teclas
    [SerializeField] KeyCode tecla;
    [SerializeField] Image fondo;
    [SerializeField] Color normal = new Color(1f, 1f, 1f, 0.15f);
    [SerializeField] Color activa = new Color(1f, 0.84f, 0f, 0.9f);
    //van a ser mostrar la tecla cuando se presiona
    void Update()
    {
        //es un operador ternario, condicion ? si es verdad : si es falso . esto equivale a
        /*
         if (Input.GetKey(tecla))
         fondo.color = activa;
            else
          fondo.color = normal;
         */
        fondo.color = Input.GetKey(tecla) ? activa : normal;
    }
}