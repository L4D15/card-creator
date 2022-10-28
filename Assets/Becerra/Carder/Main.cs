using Becerra.Carder;
using Becerra.Carder.Capture;
using Becerra.Carder.Page;
using Becerra.Save;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    public RectTransform cardParent;
    public CardView cardView;

    private CardParser parser;
    private SaveService saveService;
    private CaptureService captureService;
    private TextAsset[] dataFiles;
    private List<PageView> pages;

    public void Awake()
    {
        var pathToDesktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        var outputPath = System.IO.Path.Combine(pathToDesktop, "Cards");

        parser = new CardParser();
        saveService = new SaveService(outputPath);
        captureService = new CaptureService();
        pages = new List<PageView>();
        
        cardView.Initialize();

        dataFiles = Resources.LoadAll<TextAsset>("Cards");

        StartCapture();
    }

    public void StartCapture()
    {
        Capture();
    }

    private async void Capture()
    {
        pages.Clear();

        foreach (var dataFile in dataFiles)
        {
            string text = dataFile.text;
            string folder = dataFile.name;

            saveService.PrepareFolder(folder);
            CreateNewPage();

            var cards = parser.SplitIntoSeparateCards(text);

            foreach (var card in cards)
            {
                HideCard(cardView);
                ShowCard(card);

                var front = await captureService.CaptureCardFront(cardView);
                var back = await captureService.CaptureCardBack(cardView);

                saveService.SaveTexture(front, folder);
                saveService.SaveTexture(back, folder);

                if (IsCurrentPageFull())
                {
                    await SaveCurrentPage(folder);

                    CreateNewPage();
                }

                AddToCurrentPage(front, back);
            }

            // If page was not full, save as it is
            if (IsCurrentPageFull() == false)
            {
                await SaveCurrentPage(folder);
            }
        }

        Debug.Log("=========DONE==========");
        Application.Quit();
    }

    private PageView GetCurrentPage()
    {
        if (pages.Count < 1) return null;

        return pages[pages.Count - 1];
    }

    private bool IsCurrentPageFull()
    {
        var page = GetCurrentPage();

        if (page == null) return true;

        return page.IsFull;
    }

    private void AddToCurrentPage(Texture2D front, Texture2D back)
    {
        var currentPage = GetCurrentPage();

        currentPage.AddCard(front, back);
    }

    private void CreateNewPage()
    {
        var currentPage = new PageView(pages.Count, cardView);

        pages.Add(currentPage);
    }

    private async Task SaveCurrentPage(string folder)
    {
        var page = GetCurrentPage();

        await SavePage(page, folder);
    }

    private async Task SavePage(PageView page, string folder)
    {
        var front = await page.GenerateFrontTexture();
        var back = await page.GenerateBackTexture();

        saveService.SaveTexture(front, folder);
        saveService.SaveTexture(back, folder);
    }

    private void ShowCard(string cardText)
    {
        CardController card = new CardController(cardText);
        
        card.Parse(parser);
        
        cardView.Show(card.model);
    }

    private void HideCard(CardView cardView)
    {
        cardView.Hide();
    }
}
