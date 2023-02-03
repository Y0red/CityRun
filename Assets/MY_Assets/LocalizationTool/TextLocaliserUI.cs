using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocaliserUI : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] LocalizedString localized;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
  
        text.text = localized.value;

        LocalizationSystem.Instance.OnLanguage.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged()
    {
        text.text = localized.value;
    }
}
