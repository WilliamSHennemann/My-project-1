using UnityEngine;
using TMPro; // se usar TMP_InputField, descomente esta linha

public class CommandInterpreter : MonoBehaviour
{
    // Referência ao campo de entrada (arraste no Inspector)
    public TMP_InputField commandInput;

    // Objeto que a câmera vai seguir (pode ser o alvo)
    public Transform targetObject;

    // Velocidade de movimento da câmera (lerp)
    public float cameraSpeed = 2f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Adiciona um listener para quando o usuário apertar Enter
        commandInput.onSubmit.AddListener(OnSubmitCommand);
        // Ou se quiser que execute ao apertar um botão, chame a função separadamente
    }

    void OnSubmitCommand(string text)
    {
        // Exemplo de texto: "Chest: 1"
        string[] parts = text.Split(':');

        if (parts.Length == 2)
        {
            string objectName = parts[0].Trim();    // "Chest"
            string valueStr = parts[1].Trim();      // "1"

            // Tenta converter para booleano (1 = true, 0 = false)
            if (int.TryParse(valueStr, out int intValue))
            {
                bool state = intValue == 1;

                // Procura um objeto com o nome especificado na cena
                GameObject foundObject = GameObject.Find(objectName);

                if (foundObject != null)
                {
                    // Ativa ou desativa o objeto (exemplo simples)
                    foundObject.SetActive(state);

                    // Move a câmera até o objeto
                    targetObject = foundObject.transform;

                    Debug.Log($"Objeto '{objectName}' foi {(state ? "ativado" : "desativado")}.");
                }
                else
                {
                    Debug.LogWarning($"Objeto '{objectName}' não encontrado.");
                }
            }
            else
            {
                Debug.LogWarning("O valor deve ser 0 ou 1.");
            }
        }
        else
        {
            Debug.LogWarning("Formato inválido. Use 'Nome: 1' ou 'Nome: 0'.");
        }

        // Limpa o campo após o comando (opcional)
        commandInput.text = "";
    }

    void Update()
    {
        // Move a câmera suavemente até o alvo (se houver)
        if (targetObject != null)
        {
            Vector3 targetPos = new Vector3(targetObject.position.x, targetObject.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, cameraSpeed * Time.deltaTime);
        }
    }
}