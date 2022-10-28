using Becerra.Carder.Text;
using TMPro;
using UnityEngine;

namespace Becerra.Carder
{
    public class CardBodyTextView : MonoBehaviour
    {
        public TextMeshProUGUI label;
        
        public void ShowText(string text)
        {
            text = text.ToRichText();
            
            label.text = text;
            
            transform.SetAsLastSibling();
        }

        public void ApplyColorToBold(Color color)
        {
            string colorCode = ColorUtility.ToHtmlStringRGB(color);
            label.text = label.text.Replace("<b>", $"<b><color=#{colorCode}>").Replace("</b>", "</color></b>");
        }

        private struct ConversionData
        {
            public int index;
            public string text;
        }
    }
}