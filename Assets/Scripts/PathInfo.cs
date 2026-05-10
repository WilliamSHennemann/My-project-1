
using TMPro;
using UnityEngine;

public class PathInfo : MonoBehaviour
{
	// The name of the sprite associated with this object
	public string spriteName;

	// Reference to the TMP_InputField
	public TMP_InputField inputField;

	// Call this to update the spriteName from the input field
	public void UpdateSpriteNameFromInput()
	{
		if (inputField != null)
		{
			spriteName = inputField.text;
		}
	}
}
