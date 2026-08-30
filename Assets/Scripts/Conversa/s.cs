using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private GameObject caixaDeTexto;
    [SerializeField] private TMP_Text textoExibido;
    [SerializeField] private string[] linhas;
    [SerializeField] private float atrasoInicial = 5f;
    [SerializeField] private float atrasoPorCaractere = 0.03f;

    [SerializeField] private AnimationR Radio; // referência ao script de animação do mentor

    private int linhaAtual = 0;

    private void Start()
    {
        caixaDeTexto.SetActive(false);
        StartCoroutine(IniciarSequencia());
    }

    private IEnumerator IniciarSequencia()
    {
        yield return new WaitForSeconds(atrasoInicial);
        caixaDeTexto.SetActive(true);
        Radio.ComecarLoop(); // dispara a animação aqui
        yield return StartCoroutine(DigitarLinha(linhas[linhaAtual]));
    }

    private IEnumerator DigitarLinha(string linha)
    {
        textoExibido.text = "";
        foreach (char c in linha)
        {
            textoExibido.text += c;
            yield return new WaitForSeconds(atrasoPorCaractere);
        }
    }

    public void ProximaLinha()
    {
        linhaAtual++;
        if (linhaAtual < linhas.Length)
        {
            StartCoroutine(DigitarLinha(linhas[linhaAtual]));
        }
        else
        {
            caixaDeTexto.SetActive(false);
        }
    }
}