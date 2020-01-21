using UnityEngine;
using UnityEngine.UI;

//public class GameManager()
//{
//    public enum ELanguage { Français, English, Spanish }

//    public static ELanguage Language { get; set; } = ELanguage.English;
//}

/// <summary>
/// Replace the text key of the GameObject and replace it by it's content in the json file.
/// </summary>
public class LoadText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>();
        // Change Français by the language chosen by the configurator or something else.
        text.text = JSONAccessAPI.GetContentText(text.text, "English"/*GameManager.Language.ToString()*/);
    }
}
