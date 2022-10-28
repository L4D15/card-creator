using Becerra.Carder.Text;
using TMPro;
using UnityEngine;

namespace Becerra.Carder
{
    public class CardTagView : MonoBehaviour
    {
        public TextMeshProUGUI label;
        
        public void Show(string tagName)
        {
            label.text = tagName.ToRichText();
        }
    }
}