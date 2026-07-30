using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

// Coloque este script SÓ no GameManager (o objeto com DontDestroyOnLoad).
// Ele NÃO mexe na câmera diretamente -- só chama CameraFocus.Focar() na câmera
// da cena atual. Como este objeto sobrevive entre cenas, ele se reconecta
// automaticamente ao Input Field e à câmera toda vez que uma cena carrega.
public class HackTerminal : MonoBehaviour
{
    [System.Serializable]
    public class AlvoHackeavel
    {
        [Tooltip("O nome que o jogador vai digitar no terminal (ex: 'Hack Folder', 'StartButton')")]
        public string comando;

        [Tooltip("O GameObject do sprite/popup que vai aparecer/desaparecer. Pode começar DESATIVADO no Inspector.")]
        public GameObject objeto;

        [Tooltip("OPCIONAL: nome de outro comando que precisa ter sido o ÚLTIMO digitado antes deste funcionar.")]
        public string contextoNecessario;
    }

    [Header("Nome exato do Input Field na cena")]
    [Tooltip("Se não souber, deixe como está -- ele tenta achar o primeiro TMP_InputField da cena")]
    public string nomeDoInputField = "InputField (TMP)";

    [Header("Alvos Hackeáveis")]
    public List<AlvoHackeavel> alvos = new List<AlvoHackeavel>();

    private TMP_InputField commandInput;
    private CameraFocus cameraFocus;
    private string contextoAtual = "";

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ConectarNaCenaAtual();
    }

    void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        ConectarNaCenaAtual();
    }

    void ConectarNaCenaAtual()
    {
        // Remove o listener antigo antes de reconectar, pra não duplicar
        if (commandInput != null)
        {
            commandInput.onEndEdit.RemoveListener(OnCommandSubmitted);
        }

        GameObject campoObj = GameObject.Find(nomeDoInputField);
        commandInput = campoObj != null ? campoObj.GetComponent<TMP_InputField>() : FindObjectOfType<TMP_InputField>();

        cameraFocus = FindObjectOfType<CameraFocus>();

        if (commandInput != null)
        {
            commandInput.onEndEdit.AddListener(OnCommandSubmitted);
        }
        else
        {
            Debug.LogWarning("HackTerminal: não achei nenhum TMP_InputField nesta cena.");
        }

        if (cameraFocus == null)
        {
            Debug.LogWarning("HackTerminal: não achei nenhum CameraFocus nesta cena.");
        }

        PopularAlvosPadrao();
    }

    // Preenche automaticamente 'Hack Folder' -> StartButton (exige 'conceito')
    // caso ainda não exista essa entrada na lista.
    void PopularAlvosPadrao()
    {
        bool jaTem = alvos.Exists(a =>
            !string.IsNullOrEmpty(a.comando) &&
            a.comando.Trim().Equals("Hack Folder", System.StringComparison.OrdinalIgnoreCase));

        if (jaTem) return;

        GameObject startButton = null;
        foreach (Transform t in FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (t.name == "StartButton")
            {
                startButton = t.gameObject;
                break;
            }
        }

        if (startButton != null)
        {
            alvos.Add(new AlvoHackeavel
            {
                comando = "Hack Folder",
                objeto = startButton,
                contextoNecessario = "conceito"
            });
            Debug.Log("HackTerminal: 'Hack Folder' configurado automaticamente para abrir StartButton.");
        }
        else
        {
            Debug.LogWarning("HackTerminal: não encontrei nenhum objeto chamado 'StartButton' nesta cena.");
        }
    }

    void OnCommandSubmitted(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;

        string[] parts = text.Split(':');
        if (parts.Length != 2)
        {
            Debug.LogWarning("Formato inválido. Use 'Nome: 1' ou 'Nome: 0'.");
            LimparCampo();
            return;
        }

        string nomeComando = parts[0].Trim();
        string valorStr = parts[1].Trim();

        if (!int.TryParse(valorStr, out int valor) || (valor != 0 && valor != 1))
        {
            Debug.LogWarning("O valor deve ser 0 ou 1.");
            LimparCampo();
            return;
        }

        bool estado = valor == 1;

        AlvoHackeavel alvo = alvos.Find(a =>
            !string.IsNullOrEmpty(a.comando) &&
            a.comando.Trim().Equals(nomeComando, System.StringComparison.OrdinalIgnoreCase));

        if (alvo != null && !string.IsNullOrEmpty(alvo.contextoNecessario))
        {
            if (!contextoAtual.Equals(alvo.contextoNecessario, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning($"Você precisa estar em '{alvo.contextoNecessario}' antes de usar '{nomeComando}'.");
                LimparCampo();
                return;
            }
        }

        GameObject objetoAlvo = (alvo != null) ? alvo.objeto : null;

        if (objetoAlvo == null)
        {
            objetoAlvo = GameObject.Find(nomeComando);
        }

        if (objetoAlvo == null)
        {
            Debug.LogWarning($"NÃO ACHEI o objeto: {nomeComando}");
            LimparCampo();
            return;
        }

        objetoAlvo.SetActive(estado);
        Debug.Log($"'{nomeComando}' foi {(estado ? "ativado" : "desativado")}.");

        contextoAtual = estado ? nomeComando : "";

        if (estado && cameraFocus != null)
        {
            cameraFocus.Focar(objetoAlvo);
        }

        LimparCampo();
    }

    void LimparCampo()
    {
        if (commandInput == null) return;
        commandInput.text = "";
        commandInput.ActivateInputField();
    }
}   