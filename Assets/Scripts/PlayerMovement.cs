
using TMPro;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    // Store all sprite objects in the scene
    private GameObject[] spriteObjects;

    // Reference to InfoPath (assign in Inspector)
    public InfoPath InfoPath;

    void Start()
    {
        // Find all GameObjects with a SpriteRenderer (using latest non-obsolete API)
        spriteObjects = GameObject.FindObjectsByType<SpriteRenderer>()
            .Select(sr => sr.gameObject).ToArray();
    }

    // Call this from a UI Button or TMP_InputField OnEndEdit event
    public void MovePlayerToInputSprite()
    {
        if (InfoPath != null)
        {
            string targetName = InfoPath.inputField != null ? InfoPath.inputField.text : InfoPath.spriteName;
            MoveToSpriteByName(targetName);
        }
        else
        {
            Debug.LogWarning("InfoPath reference not set in PlayerMovement.");
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
