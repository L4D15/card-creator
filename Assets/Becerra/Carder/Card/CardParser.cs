
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Becerra.Carder
{
    public class CardParser
    {
        public CardParser()
        {

        }

        public CardModel Parse(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            
            string id = string.Empty;
            CardModel model = new CardModel();

            string[] rawSections = text.Split('\n');
            bool isMetadataFinished = false;

            for (int i = 0; i < rawSections.Length; i++)
            {
                string sectionText = rawSections[i];

                if (isMetadataFinished == false && IsMetadata(sectionText))
                {
                    ApplyMetadata(sectionText, model);
                }
                else
                {
                    isMetadataFinished = true;
                    var section = ParseSection(rawSections[i]);

                    if (section != null)
                    {
                        model.sections.Add(section);
                    }
                }
            }
            
            return model;
        }

        public IEnumerable<string> SplitIntoSeparateCards(string rawText)
        {
            List<string> cards = new List<string>();
            string[] split = rawText.Split('\n');
            List<string> accumulatedSections = new List<string>();

            for (int i = 0; i < split.Length; i++)
            {
                string section = split[i];

                if (IsNameMetadata(section) && i > 0)
                {
                    string card = MergeSectionsIntoCard(accumulatedSections);

                    if (string.IsNullOrEmpty(card) == false)
                    {
                        cards.Add(card);
                    }
                    
                    // Start a new card
                    accumulatedSections.Clear();
                    accumulatedSections.Add(section);
                }
                else if (i == split.Length - 1)
                {
                    // Merge all accumulated sections into one card
                    accumulatedSections.Add(section);
                    
                    string card = MergeSectionsIntoCard(accumulatedSections);

                    if (string.IsNullOrEmpty(card) == false)
                    {
                        cards.Add(card);
                    }
                }
                else
                {
                    accumulatedSections.Add(section);
                }
            }

            return cards;
        }

        private string MergeSectionsIntoCard(IEnumerable<string> sections)
        {
            string card = string.Empty;

            foreach (var section in sections)
            {
                if (section == "\n" || section == "\r") continue;
                card += section.Trim() + "\n";
            }

            card.Replace(@"\r", @"\n");
            
            return card;
        }

        private CardSection ParseSection(string text)
        {
            if (string.IsNullOrEmpty(text)) return null;

            CardSection section;

            if (CardSectionSeparator.IsOfType(text))
            {
                section = new CardSectionSeparator(text);
            }
            else if (CardSectionActionTitle.IsOfType(text))
            {
                section = new CardSectionActionTitle(text);
            }
            else if (CardSectionListItem.IsOfType(text))
            {
                section = new CardSectionListItem(text);
            }
            else
            {
                section = new CardSectionText(text);
            }
            
            return section;
        }

        private bool IsMetadata(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (IsNameMetadata(text)) return true;

            string[] split = text.Split(':');

            if (split.Length < 2) return false;
            if (split.Length > 1) return true;

            return false;
        }

        private void ApplyMetadata(string text, CardModel model)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (IsNameMetadata(text))
            {
                model.name = GetNameMetadata(text);
            }
            else
            {
                string key = GetMetadataKey(text);
                string value = GetMetadataValue(text);

                if (string.IsNullOrEmpty(key)) return;
                if (string.IsNullOrEmpty(value)) return;

                switch (key)
                {
                    case "Categoría":
                    {
                        model.categories = GetValuesList(value);
                        break;
                    }
                    case "Etiquetas":
                    {
                        model.tags = GetValuesList(value);
                        break;
                    }
                    case "Frontal":
                    {
                        model.frontImage = value;
                        break;
                    }
                    case "Trasera":
                    {
                        model.backImage = value;
                        break;
                    }
                    case "Fuente":
                    {
                        model.source = value;
                        break;
                    }
                    case "Nivel":
                    {
                        model.level = value;
                        break;
                    }
                    case "Acciones":
                    {
                        model.actions = value;
                        break;
                    }
                    default:
                    {
                        if (model.metadata.ContainsKey(key))
                        {
                            Debug.LogError($"Key {key} already present in generic metadata.");
                        }
                        else
                        {
                            model.metadata.Add(key, value);
                        }

                        break;
                    }
                }
            }
        }

        private bool IsNameMetadata(string text)
        {
            return text.Contains("#") && text.Contains("##") == false;
        }

        private string GetNameMetadata(string text)
        {
            var regex = new Regex("(?<=# ).*");
            var match = regex.Match(text);

            return match.Value;
        }

        private string GetMetadataKey(string text)
        {
            var regex = new Regex(".*(?=: )");
            var match = regex.Match(text);

            return match.Value;
        }
        
        private string GetMetadataValue(string text)
        {
            var regex = new Regex("(?<=: ).*");
            var match = regex.Match(text);

            return match.Value;
        }

        public static List<string> GetValuesList(string text)
        {
            var trimmedText = TrimValuesList(text);

            return new List<string>(trimmedText.Split(','));
        }
        
        public static string TrimValuesList(string text)
        {
            var regex = new Regex(@"(?<=,)\s");

            return regex.Replace(text, string.Empty);
        }
    }
}