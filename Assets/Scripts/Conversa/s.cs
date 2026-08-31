using System.Collections;
using UnityEngine;
using TMPro;

public enum TipoDeGatilho { Clique, AoAtivar }

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TipoDeGatilho gatilho = TipoDeGatilho.Clique;
    [SerializeField] private GameObject caixaDeTexto;
    [SerializeField] private TMP_Text textoExibido;
    [SerializeField] private string[] linhas;
    [SerializeField] private float atrasoInicial = 5f;
    [SerializeField] private float atrasoPorCaractere = 0.03f;
    [SerializeField] private float atrasoParaSumir = 5f;

    [Tooltip("Só usado se o gatilho for Clique e precisar ligar uma animação junto")]
    [SerializeField] private AnimationR Radio;

    private int linhaAtual = 0;
    private bool radioLigado = false;
    private bool dialogoAtivado = false;

    private void Start()
    {
        if (gatilho == TipoDeGatilho.Clique)
        {
            caixaDeTexto.SetActive(false);

            if (Radio != null)
                StartCoroutine(LigarRadioAposDelay());
            else
                radioLigado = true;
        }
    }

    private void OnEnable()
    {
        if (gatilho == TipoDeGatilho.AoAtivar && !dialogoAtivado)
        {
            dialogoAtivado = true;
            StartCoroutine(MostrarEEsconder());
        }
    }

    private IEnumerator LigarRadioAposDelay()
    {
        yield return new WaitForSeconds(atrasoInicial);
        Radio.ComecarLoop();
        radioLigado = true;
    }

    private void OnMouseDown()
    {
        if (gatilho != TipoDeGatilho.Clique) return;
        if (!radioLigado || dialogoAtivado) return;
        dialogoAtivado = true;
        StartCoroutine(MostrarEEsconder());
    }

    private IEnumerator MostrarEEsconder()
    {
        caixaDeTexto.SetActive(true);
        yield return StartCoroutine(DigitarLinha(linhas[linhaAtual]));
        yield return new WaitForSeconds(atrasoParaSumir);
        caixaDeTexto.SetActive(false);
        textoExibido.text = "";
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