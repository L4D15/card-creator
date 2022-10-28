using UnityEngine;

namespace Becerra.Carder
{
    public class CardSeparatorView : MonoBehaviour
    {
        public void Show(CardSectionSeparator separator)
        {
            transform.SetAsLastSibling();
        }
    }
}