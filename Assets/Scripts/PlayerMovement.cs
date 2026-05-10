
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Store all sprite objects in the scene
    private GameObject[] spriteObjects;

    // Reference to PathInfo (assign in Inspector)
    public PathInfo pathInfo;

    void Start()
    {
        // Find all GameObjects with a SpriteRenderer
        spriteObjects = GameObject.FindObjectsOfType<SpriteRenderer>()
            .Select(sr => sr.gameObject).ToArray();
    }

    // Call this from a UI Button or TMP_InputField OnEndEdit event
    public void MovePlayerToInputSprite()
    {
        if (pathInfo != null)
        {
            string targetName = pathInfo.inputField != null ? pathInfo.inputField.text : pathInfo.spriteName;
            MoveToSpriteByName(targetName);
        }
        else
        {
            Debug.LogWarning("PathInfo reference not set in PlayerMovement.");
        }
    }

    void MoveToSpriteByName(string name)
    {
        foreach (var obj in spriteObjects)
        {
            if (obj.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                transform.position = obj.transform.position;
                Debug.Log($"Moved to sprite: {name}");
                return;
            }
        }
        Debug.LogWarning($"Sprite with name '{name}' not found.");
    }
}
