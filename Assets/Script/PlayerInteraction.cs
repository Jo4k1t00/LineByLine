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

    public bool Mirando { get; private set; }
    public event System.Action AlInteractuar;

    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out var hit, _rayDistance, layermask)
            && hit.collider.TryGetComponent(out IInteractable obj))
        {
            Mirando = true;
            Sprite.material = Rosado;

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                obj.Interact();
                AlInteractuar?.Invoke();
            }
        }
        else
        {
            Mirando = false;
            Sprite.material = Blanco;
        }
    }
}