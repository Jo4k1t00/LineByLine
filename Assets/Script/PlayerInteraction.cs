using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Image Sprite;
    [SerializeField] Camera cam;          // arrastra aquí tu cámara
    [SerializeField] Material Rosado;
    [SerializeField] Material Blanco;
    [SerializeField] float _rayDistance = 4f;
    [SerializeField] LayerMask layermask;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out var hit, _rayDistance, layermask)
            && hit.collider.TryGetComponent(out IInteractable obj))
        {
            Sprite.material = Rosado;
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) obj.Interact();
        }
        else
        {
            Sprite.material = Blanco;
        }
    }
}