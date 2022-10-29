using Becerra.Carder.Text;
using TMPro;
using UnityEngine;

namespace Becerra.Carder.Card
{
    public class MetadataEntryView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _key;
        [SerializeField] private TextMeshProUGUI _label;

        public void Show(string key, string value)
        {
            _key.text = key.ToRichText();
            _label.text = value.ToRichText();
        }
    }
}