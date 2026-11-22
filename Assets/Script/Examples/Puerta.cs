using UnityEngine;

public class Puerta : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        Debug.Log("Abriendo Puerta...");
    }

    public string GetDescription()
    {
        return "Presiona E para abrir";
    }
}
