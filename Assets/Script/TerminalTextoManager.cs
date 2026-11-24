using System.Text;
using TMPro;
using UnityEngine;

//hace que unity la muestre en el inspector como una lista editable
[System.Serializable]
public class PasoExplicacion
{
    public int linea;                 // índice de línea del .txt (empieza en 0)
    public string fragmento;          // parte de la línea a destacar (vacío = toda la línea)
    [TextArea(2, 5)] public string texto; //que texto mostrar
    //da una caja de texto de varias lineas en vez de una sola.
}
//este es el que dibuja.
public class TerminalTextoManager : MonoBehaviour
{
    //el texto dondse muestra el codigo
    [Header("Código")]
    public TextMeshProUGUI displayText;
    //el texto donde se explica, el panel que lo contiene
    [Header("Explicación")]
    public TextMeshProUGUI explanationText;
    public GameObject explanationPanel;
    //indica si hay una simulacion corriendo, para bloquear los botones de paso mientras tanto
    public bool Ocupado { get; set; }   // true mientras corre la simulación

    //etiquetas mark de TMP
    //Pintan un fonso detras del texto, los ultimos dos digitos hex son la transparencia
    const string MarkLinea = "<mark=#FFFFFF30>";//blanco para la linea
    const string MarkFragmento = "<mark=#FFD70066>";//dorado para el fragmento

    //el codigo original separado linea por linea
    string[] rawLines;
    //lista de explicaciones
    PasoExplicacion[] pasos;
    // significa "todavia ningun paso"
    int pasoActual = -1;

    
    public void SetCode(string raw)
    { 
        //Quitar retorno de carro de windows, si no, ensucian el texto
        rawLines = raw.Replace("\r", "").Split('\n');//split corta el texto en un arreglo, una posicion por linea 
        Render(-1, null);//dibuja todo sin resaltar nada -1 = ninguna linea
    }

    // atajos que llaman a render. "=>" es una fotma corta de escribir un metodo de una sola linea
    public void Highlight(int linea, string fragmento = null) => Render(linea, fragmento);
    public void ClearHighlight() => Render(-1, null);

    //render este es el corazon
    void Render(int linea, string fragmento)
    {
        if (rawLines == null) return;
        //stringbuilder arma texto largo de forma eficiente, mas rapido que sumar strings.
        var sb = new StringBuilder();

        for (int i = 0; i < rawLines.Length; i++)
        {
            string l = rawLines[i];

            // Recorre cada linea, si es la linea a resaltar y hay un fragmento
            if (i == linea && !string.IsNullOrEmpty(fragmento))
            {
                //indexof busca donde empieza el fragmento dentro de la linea
                int idx = l.IndexOf(fragmento);
                //insert mete 2 marcadores temporales invisibles, se inserta primero el final y luefo el inicio
                // esto para que la posicion idx no se corra
                if (idx >= 0)
                    l = l.Insert(idx + fragmento.Length, "\u0002").Insert(idx, "\u0001");
            }

            //esto colores la linea, si cambia los marcadores por las etiquetas mark reales
            //el final cierra el dorado y reabre el blanco, para que el resto de la linea siga con su fondo de linea
            string f = SyntaxHighlighter.FormatCode(l);
            f = f.Replace("\u0001", MarkFragmento).Replace("\u0002", "</mark>" + MarkLinea);

            //si es la linea resaltada, la envuelve en el fondo blanco
            if (i == linea) f = MarkLinea + f + "</mark>";
            sb.Append(f);// agrega la linea al resultado
            //salto de lineas
            if (i < rawLines.Length - 1) sb.Append('\n');
        }
        //finalmente pone el texto armado en pantalla
        displayText.text = sb.ToString();
    }

    //Sistema de pasos, este guarda la lista de pasos y reinicia contador
    public void SetPasos(PasoExplicacion[] p)
    {
        pasos = p;
        pasoActual = -1;
        explanationText.text = "";
    }

    public void SiguientePaso()
    {
        //si esta ocupado o no hay pasos, sale sin hacer nada, devuelve el metodo sin hacer nada
        if (Ocupado || pasos == null || pasos.Length == 0) return;
        //avanza uno pero math evita pasarse al ultimo
        pasoActual = Mathf.Min(pasoActual + 1, pasos.Length - 1);
        MostrarPaso();
    }

    public void PasoAnterior()
    {
        //lo mismo pero al reves, solo que ahora el paso anterior.
        if (Ocupado || pasos == null || pasos.Length == 0) return;
        pasoActual = Mathf.Max(pasoActual - 1, 0);
        MostrarPaso();
    }

    void MostrarPaso()
    {
        //Toma el paso actual, ilumina su linea y escribe la explicacion con <b> que es negrita
        //suma 1 para mostrar el paso 1 aunque el indice diga que es 0
        var p = pasos[pasoActual];
        Highlight(p.linea, p.fragmento);
        explanationText.text = $"<b>Paso {pasoActual + 1}/{pasos.Length}</b>\n{p.texto}";
    }

    public void ToggleExplanation()
    {
        //alterna el panel igual que el terminal interactiva
        bool abrir = !explanationPanel.activeSelf;
        explanationPanel.SetActive(abrir);

        //al abrir si es la primera vez, muestra el paso 1, si ya avanzo antes, muestra el que quedo
        if (abrir)
        {
            if (pasoActual < 0) SiguientePaso();
            else if (!Ocupado) MostrarPaso();
        }
        else if (!Ocupado)
        {
            // al cerrar quita el resultado
            ClearHighlight();
        }
    }

    // propiedad de solo lectura: true si el panel esta abierto. plauer code view lo consulta para saber quien manda
    public bool ModoManual => explanationPanel != null && explanationPanel.activeSelf;

    // Ilumina varias líneas a la vez. Cada bit de la máscara es una línea (bit 3 = línea 3)
    public void HighlightMask(int mascara)
    {
        if (rawLines == null) return;
        var sb = new StringBuilder();

        for (int i = 0; i < rawLines.Length; i++)
        {
            string f = SyntaxHighlighter.FormatCode(rawLines[i]);
            //este es como render, pero en vez de uns sola linea ilumina varias.
            //usa una mascara de bits: un numero entero donde cada bit de 0 a 1 representa una linea
            if ((mascara & (1 << i)) != 0) f = MarkLinea + f + "</mark>";
            sb.Append(f);
            if (i < rawLines.Length - 1) sb.Append('\n');
        }
        displayText.text = sb.ToString();
    }
}