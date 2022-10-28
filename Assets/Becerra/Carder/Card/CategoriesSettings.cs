using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Becerra.Carder.Card
{
    [CreateAssetMenu(fileName = "CategoriesSetup", menuName = "Becerra/Categories Setup")]
    public class CategoriesSettings : ScriptableObject
    {
        [System.Serializable]
        public class CategorySetup
        {
            public string name;
            public Sprite icon;
            public Color color;
        }

        [SerializeField]
        private List<CategorySetup> _categories;

        public CategorySetup FindSetup(string categoryName)
        {
            var setup = _categories.FirstOrDefault(s => s.name == categoryName);

            if (setup == null) return _categories.Find(s => s.name == "Default");

            return setup;
        }
    }
}
