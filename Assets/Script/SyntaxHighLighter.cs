using System.Text.RegularExpressions;
using UnityEngine;

//static significa que no necesitas crear un objeto ni ponerlo en la escena.
public static class SyntaxHighlighter//lo llamarias directo con .FormatCode(...)
{
    //colores en hexadecimal, adaptados en el color visualstudio
    //const es un valor fijo que no cambia.
    private const string KeywordColor = "#569CD6"; // Azul para public, class, void
    private const string TypeColor = "#4EC9B0";    // Verde claro para MonoBehaviour, Transform, etc.

    public static string FormatCode(string rawCode)
    {
        //recibe una linea de codigo en texto plano y devuelve la misma linea con etiquetas de color
        string formattedCode = rawCode;

        //lista de palabras clave en string y devuelve las palabras clave con un color.
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

        //ojo el color 4EC9B0 contiene letras y numeros si algundia agregas una palabra clave que coincida con parte de la etiqueta podria romper el formato
    }
}
