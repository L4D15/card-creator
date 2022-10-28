using Becerra.Carder.Text;
using TMPro;
using UnityEngine;

namespace Becerra.Carder
{
    public class CardCategoryView : MonoBehaviour
    {
        public TextMeshProUGUI label;
        
        public void Show(string categoryName)
        {
            label.text = categoryName.ToRichText();
        }
    }
}