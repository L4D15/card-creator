using Becerra.Carder.Text;
using TMPro;
using UnityEngine;

namespace Becerra.Carder
{
    public class CardActionTitleView : MonoBehaviour
    {
        public TextMeshProUGUI label;
        
        public void Show(string titleName)
        {
            label.text = titleName.ToRichText();
            
            transform.SetAsLastSibling();
        }
    }
}