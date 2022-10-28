using NUnit.Framework;
using UnityEngine;

 namespace Becerra.Carder.Tests
 {
     public class Parser
     {
        public CardParser parser;
        public string fullCardText = "\n# CARD_NAME\n\nCategories: CATEGORY_A, CATEGORY_B\nTags: TAG_A, TAG_B\nSource: SOURCE_ID\nFront: FRONT_IMAGE\nBack: BACK_IMAGE\n\nThis is a card doing stuff. Cool uh?\n---\n\n## ACTION_TITLE; 1,2,3,r,f\n---\n__Requisite:__ I need some stuff to work.\n---\nThis is the stuff I do.\n---\n\nBut I can do more things, like all of this:\n- Wo\n- lo\n- lo\n- You are converted.\n\nI can even print text in __bold__, **italic** or __**bold italic too**__.";

        [SetUp]
        public void Setup()
        {
            parser = new CardParser();
        }

        [Test]
        public void ParseEmpty()
        {
             string text = string.Empty;
             CardModel model = parser.Parse(text);

             Assert.IsNull(model);
        }
        
        [Test]
        public void Name()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);
            
            Assert.IsNotNull(model);
            Assert.AreEqual("CARD_NAME", model.name);
        }

        [Test]
        public void FrontImage()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);
            
            Assert.AreEqual("FRONT_IMAGE", model.frontImage);
        }

        [Test]
        public void BackImage()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);

            Assert.AreEqual("BACK_IMAGE", model.backImage);
        }

        [Test]
        public void Categories()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);
            
            Assert.IsTrue(model.categories.Contains("CATEGORY_A"));
            Assert.IsTrue(model.categories.Contains("CATEGORY_B"));
        }

        [Test]
        public void Tags()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);
            
            Assert.IsTrue(model.tags.Contains("TAG_A"));
            Assert.IsTrue(model.tags.Contains("TAG_B"));
        }

        [Test]
        public void Source()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);
            
            Assert.AreEqual("SOURCE_ID", model.source);
        }

        [Test]
        public void Separator()
        {
            string text = "---";
            CardModel model = parser.Parse(text);
            
            Assert.IsTrue(model.sections[0] is CardSectionSeparator);
        }

        [Test]
        public void ActionTitle()
        {
            string text = "## ACTION_TITLE; 1,2,3,r,f";
            CardModel model = parser.Parse(text);

            var section = model.sections[0];
            var titleSection = section as CardSectionActionTitle;
            
            Assert.IsNotNull(section);
            Assert.IsNotNull(titleSection);
            Assert.AreEqual("ACTION_TITLE", titleSection.Title);
        }

        [Test]
        public void List()
        {
            string text = "- Item A\n- Item B";
            CardModel model = parser.Parse(text);

            var itemA = model.sections[0] as CardSectionListItem;
            var itemB = model.sections[1] as CardSectionListItem;
            
            Assert.IsNotNull(itemA);
            Assert.IsNotNull(itemB);
            Assert.AreEqual("Item A", itemA.Text);
            Assert.AreEqual("Item B", itemB.Text);
        }

        [Test]
        public void Full()
        {
            string text = fullCardText;
            CardModel model = parser.Parse(text);

            Assert.AreEqual("CARD_NAME", model.name);
            Assert.AreEqual("SOURCE_ID", model.source);
            Assert.AreEqual("FRONT_IMAGE", model.frontImage);
            Assert.AreEqual("BACK_IMAGE", model.backImage);
            
            var firstSection = model.sections[0] as CardSectionText;
            var secondSection = model.sections[1] as CardSectionSeparator;
            var thirdSection = model.sections[2] as CardSectionActionTitle;
            var fourthSection = model.sections[3] as CardSectionSeparator;
            
            
            Assert.IsNotNull(firstSection);
            Assert.IsNotNull(secondSection);
            Assert.IsNotNull(thirdSection);
            Assert.IsNotNull(fourthSection);
        }
     }
 }