using UnityEngine;
using UnityEngine.UI;

public class TeclaIndicador : MonoBehaviour
{
    [SerializeField] KeyCode tecla;
    [SerializeField] Image fondo;
    [SerializeField] Color normal = new Color(1f, 1f, 1f, 0.15f);
    [SerializeField] Color activa = new Color(1f, 0.84f, 0f, 0.9f);

    void Update()
    {
        fondo.color = Input.GetKey(tecla) ? activa : normal;
    }
}