using UnityEngine;
using TMPro;

public class CameraSearch : MonoBehaviour
{
    [Header("Arraste seu Input Field aqui")]
    public TMP_InputField searchField;

    [Header("Configurações de Movimento")]
    public float smoothSpeed = 0.125f;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);

    [Header("Ajuste de Altura (Opcional)")]
    [Tooltip("Aumente este valor para a câmera subir um pouco mais além do centro")]
    public float ajusteSubida = 0.5f;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;

        if (searchField != null)
        {
            searchField.onEndEdit.AddListener(delegate { SearchForTarget(searchField.text); });
        }
        else
        {
            Debug.LogError("ERRO: Você esqueceu de arrastar o Input Field para a câmera!");
        }
    }

    public void SearchForTarget(string name)
    {
        if (string.IsNullOrEmpty(name)) return;

        GameObject foundObject = GameObject.Find(name);

        if (foundObject != null)
        {
            Vector3 visualCenter;

            // Tenta pegar o centro do Sprite/Renderizador para não focar nos "pés" do objeto
            if (foundObject.TryGetComponent<Renderer>(out Renderer renderer))
            {
                visualCenter = renderer.bounds.center;
            }
            else
            {
                visualCenter = foundObject.transform.position;
            }

            // Define o alvo: Centro visual + o ajuste de subida + a profundidade da câmera (Z)
            targetPosition = new Vector3(visualCenter.x, visualCenter.y + ajusteSubida, cameraOffset.z);

            isMoving = true;
            Debug.Log($"Alvo encontrado: {name}. Centralizando no sprite!");
        }
        else
        {
            Debug.LogWarning("NÃO ACHEI o objeto: " + name);
        }
    }

    void LateUpdate()
    {
        if (isMoving)
        {
            // Movimento suave até o destino
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            // Para de mover quando estiver bem perto para economizar processamento
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}