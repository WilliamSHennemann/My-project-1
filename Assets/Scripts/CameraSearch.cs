using UnityEngine;
using TMPro;

public class CameraSearch : MonoBehaviour
{
    [Header("Arraste seu Input Field aqui")]
    public TMP_InputField searchField;

    [Header("Configurações")]
    public float smoothSpeed = 0.1f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        targetPosition = transform.position;

        // É esta linha que faz o Enter funcionar automaticamente!
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
            targetPosition = new Vector3(foundObject.transform.position.x, foundObject.transform.position.y, offset.z);
            isMoving = true;
            Debug.Log("ACHEI! Movendo câmera para: " + name);
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
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}