using UnityEngine;

public class PlayerCodeView : MonoBehaviour
{
    [SerializeField] TerminalTextoManager terminal;
    [SerializeField] TextAsset codigo;
    [SerializeField] PasoExplicacion[] pasos;
    [SerializeField] CharacterController controller;
    [SerializeField] PlayerInteraction interaccion;

    // Un bit por línea del Player.txt, cada constante es una mascara
    //<< es desplazamiento, este en resumen recorre del 1 al 4 
    // | es combinar o OR, es como juntar interruptores si cualquiera de estos tiene lo que busca, el resultado es positivo
    const int MOVER = (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6);
    const int SALTAR = (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12) | (1 << 13) | (1 << 14) | (1 << 15);
    const int MIRAR = (1 << 18) | (1 << 19);
    const int APUNTAR = (1 << 22) | (1 << 23);
    const int INTERACTUAR = (1 << 24) | (1 << 25);

    //OJO si cambiamos .txt, se tendra que cambiar estos numeros de linea para que coincidan.

    //Asi un arreglo de 32 numeros, uno por linea. Guarda hasta que segundo debe seguir encendida cada linea.
    //hasta el 3 significa que la linea 3 esta encendida hasta el segundo 12.5
    readonly float[] hasta = new float[32];   // hasta cuándo sigue encendida cada línea
    //recuerda la ultima mascara dibujada, para no redibujar si nada cambio. Empieza en -1 (un valor imposible, asi que le primer frame siempre dibuja)
    int ultimaMascara = -1;
   //readonly la variable hasta no puede reasignarse a otro arreglo, pero sus valores internos si cambian


    // Este convoca el evento AlInteractuar de player interaction
    //OnEnable se ejecuta al activarse el script, += significa suscribeme, cuando el jugador interactue llamara a OnInteractuar
    void OnEnable() { interaccion.AlInteractuar += OnInteractuar; }
    //OnDisable al desactivarse -= da de baja. es importante para no dejaer suscripciones colgadas que dejen errores
    void OnDisable() { interaccion.AlInteractuar -= OnInteractuar; }
    // OnInteractuar cuando llega el aviso enciende las lineas de interactuar por 0.6 segundos
    void OnInteractuar() => Encender(INTERACTUAR, 0.6f);

    void Start()
    {
        //Carga el texto del .txt en la terminal y le pasa la lista de pasos de explicacion, es lo que usa el terminaltextomanager
        terminal.SetCode(codigo.text);
        terminal.SetPasos(pasos);
    }

    void Update()
    {
        // Con el panel "?" abierto manda la explicación manual
        if (terminal.ModoManual) { ultimaMascara = -1; return; }

        //Cada if detecta una accion y enciende su grupo por un tiempo
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)// si te mueves 
            Encender(MOVER, 0.2f);
        if (!controller.isGrounded || Input.GetButton("Jump"))//si saltas
            Encender(SALTAR, 0.3f);
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f)//si el mouse se mueve, abs quita el signo
            Encender(MIRAR, 0.2f);
        if (interaccion.Mirando)//si el rato esta sobre algo interactuable
            Encender(APUNTAR, 0.1f);

        //Los tiempos entre .1 a .3 sirven para que no parpadee

        int mascara = 0;
        //esto es leer los tiempos y contruir el nuemro
        for (int i = 0; i < 32; i++)
            if (Time.time < hasta[i]) mascara |= 1 << i;//agrega el interruptor a lo que ya tengo

        // Solo redibuja cuando cambia, para no regenerar el texto cada frame
        if (mascara != ultimaMascara)
        {
            ultimaMascara = mascara;
            terminal.HighlightMask(mascara);
        }
    }

    //la mascara y el segundo que tarda en segundos
    void Encender(int mascara, float segundos)
    {
        //revisa las 32 posiciones, una por una
        for (int i = 0; i < 32; i++)//para cada mascara & el recorrido de 1 a i preguntara ¿la linea i esta en este grupo?
            if ((mascara & (1 << i)) != 0) hasta[i] = Time.time + segundos;//si lo tiene, anota hasta [i]= 10 + 0.2 = 10.2: esta linea queda encendida hasta el segundo 10.2
    }

    //con MOVER por ejemplo, solo las posiciones 3,4,5 y 6 pasan de filtro, asi que slo esas se actualizan. las otras quedan como que no existen
}