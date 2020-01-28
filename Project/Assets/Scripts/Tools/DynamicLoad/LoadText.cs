using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Replace the text key of the GameObject and replace it by it's content in the json file.
/// </summary>
public class LoadText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private string key;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        key = text.text;
        // Change Français by the language chosen by the configurator or something else.
    }

    private void Update()
    {
        text.text = JSONAccessAPI.GetContentText(key, GameParameter.Language.ToString());       
    }
}
