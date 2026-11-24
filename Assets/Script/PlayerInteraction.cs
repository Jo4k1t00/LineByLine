using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Image Sprite;
    [SerializeField] Camera cam;
    [SerializeField] Material Rosado;
    [SerializeField] Material Blanco;
    [SerializeField] float _rayDistance = 4f;
    [SerializeField] LayerMask layermask;

    //mirando es un boll que otros scripts pueden leer pero no cambiar
    public bool Mirando { get; private set; }
    //es un evento, como un aviso
    // es como decir "Interactuar" y quien este escuchando reacciona.
    public event System.Action AlInteractuar;

    void Update()
    {
        //crea un rayo
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        //si colisiona con este rayo con su layer y tiene un componente que cumpla IInteractable
        if (Physics.Raycast(ray, out var hit, _rayDistance, layermask)
            && hit.collider.TryGetComponent(out IInteractable obj))
        {
            //mirando es verdadero
            Mirando = true;
            //cambio de color
            Sprite.material = Rosado;

            //si presionas el clic o E
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                //Interactuas con el objeto llamando al metodo de su objeto
                obj.Interact();
                //dispara el evento. ? significa "solo si alguien esta suscrito"
                //sin el, daria error cuando nadie escucha
                AlInteractuar?.Invoke();
            }
        }
        else
        {
            //simplemetne no pasa nada el color sigue en blanco 
            Mirando = false;
            Sprite.material = Blanco;
        }
    }
}