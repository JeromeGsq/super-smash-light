using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Replace the text key of the GameObject and replace it by it's content in the json file.
/// </summary>
public class LoadAudioText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = JSONAccessAPI.GetAudioText(text.text);
    }
}
