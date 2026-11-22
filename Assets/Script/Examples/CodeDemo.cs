using System.Collections;
using UnityEngine;

public abstract class CodeDemo : MonoBehaviour, IInteractable
{
    [SerializeField] protected TerminalTextoManager terminal;
    [SerializeField] TextAsset codigo;
    [SerializeField] PasoExplicacion[] pasos;   // la explicación manual, se escribe en el Inspector
    bool running;

    protected virtual void Start()
    {
        terminal.SetCode(codigo.text);
        terminal.SetPasos(pasos);
    }

    public void Interact()
    {
        if (!running) StartCoroutine(Wrapper());
    }

    IEnumerator Wrapper()
    {
        running = true;
        terminal.Ocupado = true;      // bloquea los botones manuales durante la simulación
        yield return Sequence();
        terminal.ClearHighlight();
        terminal.Ocupado = false;
        running = false;
    }

    protected abstract IEnumerator Sequence();

    protected IEnumerator Line(int n, float wait = 1f)
    {
        terminal.Highlight(n);
        yield return new WaitForSeconds(wait);
    }
}