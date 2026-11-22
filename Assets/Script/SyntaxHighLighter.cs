using System.Text.RegularExpressions;
using UnityEngine;

public static class SyntaxHighlighter
{
    // Colores estilo Visual Studio (puedes cambiarlos a tu gusto)
    private const string KeywordColor = "#569CD6"; // Azul para public, class, void
    private const string TypeColor = "#4EC9B0";    // Verde claro para MonoBehaviour, Transform, etc.

    public static string FormatCode(string rawCode)
    {
        string formattedCode = rawCode;

        // 1. Colorear palabras clave comunes de C#
        string[] keywords = { "public", "private", "protected", "void", "class", "return", "if", "else" };
        foreach (var kw in keywords)
        {
            // Busca la palabra exacta y la envuelve en etiquetas de color de TMP
            formattedCode = Regex.Replace(formattedCode, $@"\b{kw}\b", $"<color={KeywordColor}>{kw}</color>");
        }

        // 2. Colorear tipos de datos / clases comunes
        string[] types = { "MonoBehaviour", "Transform", "Image", "RaycastHit", "LayerMask", "string", "float", "int" };
        foreach (var t in types)
        {
            formattedCode = Regex.Replace(formattedCode, $@"\b{t}\b", $"<color={TypeColor}>{t}</color>");
        }

        return formattedCode;
    }
}
