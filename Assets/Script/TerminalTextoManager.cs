using System.Text;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PasoExplicacion
{
    public int linea;                 // índice de línea del .txt (empieza en 0)
    public string fragmento;          // parte de la línea a destacar (vacío = toda la línea)
    [TextArea(2, 5)] public string texto;
}

public class TerminalTextoManager : MonoBehaviour
{
    [Header("Código")]
    public TextMeshProUGUI displayText;

    [Header("Explicación")]
    public TextMeshProUGUI explanationText;
    public GameObject explanationPanel;

    public bool Ocupado { get; set; }   // true mientras corre la simulación

    const string MarkLinea = "<mark=#FFFFFF30>";
    const string MarkFragmento = "<mark=#FFD70066>";

    string[] rawLines;
    PasoExplicacion[] pasos;
    int pasoActual = -1;

    // ---------- Código ----------
    public void SetCode(string raw)
    {
        rawLines = raw.Replace("\r", "").Split('\n');
        Render(-1, null);
    }

    public void Highlight(int linea, string fragmento = null) => Render(linea, fragmento);
    public void ClearHighlight() => Render(-1, null);

    void Render(int linea, string fragmento)
    {
        if (rawLines == null) return;
        var sb = new StringBuilder();

        for (int i = 0; i < rawLines.Length; i++)
        {
            string l = rawLines[i];

            // Marcadores temporales alrededor del fragmento
            if (i == linea && !string.IsNullOrEmpty(fragmento))
            {
                int idx = l.IndexOf(fragmento);
                if (idx >= 0)
                    l = l.Insert(idx + fragmento.Length, "\u0002").Insert(idx, "\u0001");
            }

            string f = SyntaxHighlighter.FormatCode(l);
            f = f.Replace("\u0001", MarkFragmento).Replace("\u0002", "</mark>" + MarkLinea);

            if (i == linea) f = MarkLinea + f + "</mark>";

            sb.Append(f);
            if (i < rawLines.Length - 1) sb.Append('\n');
        }
        displayText.text = sb.ToString();
    }

    // ---------- Explicación manual ----------
    public void SetPasos(PasoExplicacion[] p)
    {
        pasos = p;
        pasoActual = -1;
        explanationText.text = "";
    }

    public void SiguientePaso()
    {
        if (Ocupado || pasos == null || pasos.Length == 0) return;
        pasoActual = Mathf.Min(pasoActual + 1, pasos.Length - 1);
        MostrarPaso();
    }

    public void PasoAnterior()
    {
        if (Ocupado || pasos == null || pasos.Length == 0) return;
        pasoActual = Mathf.Max(pasoActual - 1, 0);
        MostrarPaso();
    }

    void MostrarPaso()
    {
        var p = pasos[pasoActual];
        Highlight(p.linea, p.fragmento);
        explanationText.text = $"<b>Paso {pasoActual + 1}/{pasos.Length}</b>\n{p.texto}";
    }

    public void ToggleExplanation()
    {
        bool abrir = !explanationPanel.activeSelf;
        explanationPanel.SetActive(abrir);

        if (abrir)
        {
            if (pasoActual < 0) SiguientePaso();
            else if (!Ocupado) MostrarPaso();
        }
        else if (!Ocupado)
        {
            ClearHighlight();
        }
    }
}