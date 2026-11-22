using UnityEngine;

public class PlayerCodeView : MonoBehaviour
{
    [SerializeField] TerminalTextoManager terminal;
    [SerializeField] TextAsset codigo;
    [SerializeField] PasoExplicacion[] pasos;
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerInteraction interaccion;

    // Un bit por línea del Player.txt
    const int MOVER = (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6);
    const int SALTAR = (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12);
    const int MIRAR = (1 << 15) | (1 << 16);
    const int APUNTAR = (1 << 19) | (1 << 20);
    const int INTERACTAR = (1 << 21) | (1 << 22);

    readonly float[] hasta = new float[32];   // hasta cuándo sigue encendida cada línea
    int ultimaMascara = -1;

    void OnEnable() { interaccion.AlInteractuar += OnInteractuar; }
    void OnDisable() { interaccion.AlInteractuar -= OnInteractuar; }
    void OnInteractuar() => Encender(INTERACTAR, 0.6f);

    void Start()
    {
        terminal.SetCode(codigo.text);
        terminal.SetPasos(pasos);
    }

    void Update()
    {
        // Con el panel "?" abierto manda la explicación manual
        if (terminal.ModoManual) { ultimaMascara = -1; return; }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            Encender(MOVER, 0.2f);
        if (!controller.isGrounded)
            Encender(SALTAR, 0.1f);
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f)
            Encender(MIRAR, 0.2f);
        if (interaccion.Mirando)
            Encender(APUNTAR, 0.1f);

        int mascara = 0;
        for (int i = 0; i < 32; i++)
            if (Time.time < hasta[i]) mascara |= 1 << i;

        // Solo redibuja cuando cambia, para no regenerar el texto cada frame
        if (mascara != ultimaMascara)
        {
            ultimaMascara = mascara;
            terminal.HighlightMask(mascara);
        }
    }

    void Encender(int mascara, float segundos)
    {
        for (int i = 0; i < 32; i++)
            if ((mascara & (1 << i)) != 0) hasta[i] = Time.time + segundos;
    }
}