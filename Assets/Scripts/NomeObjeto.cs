using UnityEngine;
using TMPro;

public class NomeDoObjeto2D : MonoBehaviour
{
    void Start()
    {
        // Busca o componente de texto que está dentro do Canvas filho
        TMP_Text texto = GetComponentInChildren<TMP_Text>();

        if (texto != null)
        {
            // Aplica o nome do objeto principal no texto
            texto.text = gameObject.name;
        }
        else
        {
            Debug.LogWarning("Nenhum componente TextMeshPro foi encontrado nos filhos de " + gameObject.name);
        }
    }
}
