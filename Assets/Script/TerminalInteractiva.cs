using UnityEngine;

// esto permite acceder al contrato, este promete que tiene el metodo Interact
public class TerminalInteractiva : MonoBehaviour, IInteractable
{
   
    public GameObject canvasTerminal;
    bool _estaAbierta = false;//si la terminal esta abierta o cerrada.

    //lo que llama cuando clickeas.
    public void Interact()
    {
        ToggleTerminal();
    }
    //Abre el booleano y enciende el objeto completo con todos los hijos.
    void ToggleTerminal()
    {
        _estaAbierta = !_estaAbierta;
        canvasTerminal.SetActive(_estaAbierta);
    }

    //OJO: el objeto 3d de la terminal no debe estar dentro del canvas si lo desactivas dentro de este, el rayo no podra interactuarlo.
}
