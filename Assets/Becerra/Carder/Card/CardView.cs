using Becerra.Carder.Card;
using Becerra.Carder.Text;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Becerra.Carder
{
    public class CardView : MonoBehaviour
    {
        //public const int WIDTH = 744;
        //public const int HEIGHT = 1039;

        [TabGroup("Categories")]
        public CategoriesSettings _categoriesSettings;

        [TabGroup("Categories")]
        public Image _frontIcon;

        [TabGroup("Categories")]
        public Image[] _colorableIcons;

        public GameObject content;

        public TextMeshProUGUI frontNameLabel;
        public TextMeshProUGUI backNameLabel;

        public GameObject levelArea;
        public TextMeshProUGUI levelLabel;

        public Image frontImage;
        public Image backImage;
        
        public Transform frontCategoriesParent;
        public CardCategoryView frontCategoryPrefab;
        
        public Transform backCategoriesParent;
        public CardCategoryView backCategoryPrefab;
        public RectTransform backIllustrationContainer;

        public Transform tagsContainer;
        public CardTagView tagPrefab;

        public TextMeshProUGUI sourceLabel;

        public Transform sectionsContainer;
        public CardBodyTextView bodyTextPrefab;
        public CardSeparatorView separatorPrefab;
        public CardActionTitleView actionTitlePrefab;
        public CardListItemView listItemPrefab;

        private Pool<CardCategoryView> frontCategoriesPool;
        private Pool<CardCategoryView> backCategoriesPool;
        private Pool<CardTagView> tagsPool;
        private Pool<CardBodyTextView> textSectionPool;
        private Pool<CardSeparatorView> separatorSectionPool;
        private Pool<CardActionTitleView> actionTitleSectionPool;
        private Pool<CardListItemView> listItemPool;
        
        public CardModel Model { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private AspectRatioFitter _frontAspectRatio;
        private AspectRatioFitter _backAspectRatio;
        
        public void Initialize()
        {
            var rect = GetComponent<RectTransform>().rect;

            this.Width = (int) (rect.width / 2);
            this.Height = (int) rect.height;

            frontCategoriesPool = new Pool<CardCategoryView>(frontCategoryPrefab, frontCategoriesParent, 4);
            backCategoriesPool = new Pool<CardCategoryView>(backCategoryPrefab, backCategoriesParent, 4);
            tagsPool = new Pool<CardTagView>(tagPrefab, tagsContainer, 6);
            textSectionPool = new Pool<CardBodyTextView>(bodyTextPrefab, sectionsContainer, 4);
            separatorSectionPool = new Pool<CardSeparatorView>(separatorPrefab, sectionsContainer, 4);
            actionTitleSectionPool = new Pool<CardActionTitleView>(actionTitlePrefab, sectionsContainer, 2);
            listItemPool = new Pool<CardListItemView>(listItemPrefab, sectionsContainer, 10);

            _frontAspectRatio = frontImage.GetComponent<AspectRatioFitter>();
            _backAspectRatio = backImage.GetComponent<AspectRatioFitter>();
        }

        public void Dispose()
        {
            frontCategoriesPool.Dispose();
            backCategoriesPool.Dispose();
            tagsPool.Dispose();
            textSectionPool.Dispose();
            separatorSectionPool.Dispose();
            actionTitleSectionPool.Dispose();
            listItemPool.Dispose();
        }
        
        public void Show(CardModel model)
        {
            Model = model;
            
            ShowName(model.name);
            ShowLevel(model.level);
            ShowFrontCategories(model.categories);
            ShowBackCategories(model.categories);
            ShowSource(model.source);
            ShowTags(model.tags);
            ShowSections(model.sections);

            string frontImage = string.IsNullOrEmpty(model.frontImage) ? model.name : model.frontImage;
            string backImage = string.IsNullOrEmpty(model.backImage) ? frontImage : model.backImage;
            
            ShowFrontImage(frontImage);
            ShowBackImage(backImage);
        }

        public void Hide()
        {
            Model = null;
            
            frontNameLabel.text = string.Empty;
            backNameLabel.text = string.Empty;
            frontImage.sprite = null;
            backImage.sprite = null;
            
            frontCategoriesPool.Reset();
            backCategoriesPool.Reset();
            tagsPool.Reset();
            textSectionPool.Reset();
            separatorSectionPool.Reset();
            actionTitleSectionPool.Reset();
            listItemPool.Reset();
        }

        private void ShowName(string name)
        {
            frontNameLabel.text = name.ToRichText();
            backNameLabel.text = name.ToRichText();
        }

        private void ShowLevel(int level)
        {
            if (level == CardModel.NO_LEVEL)
            {
                levelArea.SetActive(false);
                levelLabel.text = string.Empty;
            }
            else
            {
                levelArea.SetActive(true);
                levelLabel.text = level.ToString();
            }
        }

        private void ShowFrontCategories(IEnumerable<string> categories)
        {
            foreach (var category in categories)
            {
                var categoryView = frontCategoriesPool.Spawn();

                categoryView.Show(category);
                categoryView.label.color = GetCategoryColor(category);

                ApplyCategoryVisuals(category);
            }
        }

        private void ShowBackCategories(IEnumerable<string> categories)
        {
            foreach (var category in categories)
            {
                var categoryView = backCategoriesPool.Spawn();

                categoryView.Show(category);
                categoryView.label.color = GetCategoryColor(category);
            }
        }

        private void ShowTags(IEnumerable<string> tags)
        {
            foreach (var tag in tags)
            {
                var tagView = tagsPool.Spawn();
                
                tagView.Show(tag);
            }
        }

        private void ShowFrontImage(string imageName)
        {
            frontImage.sprite = LoadSprite(imageName);

            RefreshAspectRatio();
        }

        private void ShowBackImage(string imageName)
        {
            backImage.sprite = LoadSprite(imageName);

            RefreshAspectRatio();
        }

        [Button]
        public void RefreshAspectRatio()
        {
            var aspectRatioFitter = backImage.GetComponent<AspectRatioFitter>();

            if (aspectRatioFitter != null && backImage.sprite != null)
            {
                float aspectRation = backImage.sprite.textureRect.width / backImage.sprite.textureRect.height;
                
                aspectRatioFitter.aspectRatio = aspectRation;
            }

            aspectRatioFitter = frontImage.GetComponent<AspectRatioFitter>();

            if (aspectRatioFitter != null && frontImage.sprite != null)
            {
                float aspectRation = frontImage.sprite.textureRect.width / frontImage.sprite.textureRect.height;
                
                aspectRatioFitter.aspectRatio = aspectRation;
            }

        }

        private void ResizeBackIllustrationContainer()
        {
            var parent = backIllustrationContainer.transform.parent.GetComponent<RectTransform>();

            float fullHeight = parent.rect.height;
            float contentHeight = content.GetComponent<RectTransform>().rect.height;
            float illustrationHeight = fullHeight - contentHeight;

            backIllustrationContainer.sizeDelta = new Vector2(backIllustrationContainer.sizeDelta.x, illustrationHeight);

        }

        private Sprite LoadSprite(string imageName)
        {
            Sprite sprite = Resources.Load<Sprite>("Images/" + imageName);

            if (sprite == null)
            {
                Debug.LogError("Image " + imageName + "not found in Resources/Images. Using the default one.");
                return LoadSprite("DefaultImage");
            }
            
            return sprite;
        }

        private void ShowSource(string source)
        {
            sourceLabel.text = source.ToRichText();
        }

        private void ShowSections(IEnumerable<CardSection> sections)
        {
            var color = GetCategoryColor(Model.categories.First());

            int count = 0;

            foreach (var section in sections)
            {
                if (section is CardSectionText textSection)
                {
                    var view = textSectionPool.Spawn();
                    
                    view.ShowText(textSection.BodyText);
                    view.ApplyColorToBold(color);
                }
                else if (section is CardSectionSeparator separatorSection)
                {
                    separatorSectionPool.Spawn().Show(separatorSection);
                }
                else if (section is CardSectionActionTitle titleSection)
                {
                    actionTitleSectionPool.Spawn().Show(titleSection.Title);
                }
                else if (section is CardSectionListItem listItemSection)
                {
                    listItemPool.Spawn().Show(listItemSection.Text);
                }

                count++;
            }

            content.SetActive(count > 0);
        }

        #region Categories

        private void ApplyCategoryVisuals(string category)
        {
            var setup = _categoriesSettings.FindSetup(category);

            if (setup == null) return;

            var icon = setup.icon;
            var color = setup.color;

            _frontIcon.sprite = icon;

            foreach (var colorable in _colorableIcons)
            {
                colorable.color = color;
            }
        }

        private Color GetCategoryColor(string category)
        {
            var setup = _categoriesSettings.FindSetup(category);

            if (setup == null) return Color.white;

            return setup.color;
        }

        #endregion
    }
}