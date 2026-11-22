using UnityEngine;

public class TerminalInteractiva : MonoBehaviour, IInteractable
{
    public GameObject canvasTerminal;
    bool _estaAbierta = false;

    public void Interact()
    {
        ToggleTerminal();
    }
    void ToggleTerminal()
    {
        _estaAbierta = !_estaAbierta;
        canvasTerminal.SetActive(_estaAbierta);
    }
}
