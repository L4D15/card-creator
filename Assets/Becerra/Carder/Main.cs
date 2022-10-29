using Becerra.Carder;
using Becerra.Carder.Capture;
using Becerra.Carder.Page;
using Becerra.Carder.Provider;
using Becerra.Save;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    public RectTransform cardParent;
    public CardView cardView;
    public string cardsPath;

    private TextFileProvider _textProvider;

    private CardParser parser;
    private SaveService saveService;
    private CaptureService captureService;
    private IEnumerable<string> folderPaths;
    private List<PageView> pages;

    public void Awake()
    {
        var pathToDesktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        var outputPath = System.IO.Path.Combine(pathToDesktop, "Cards");

        _textProvider = new TextFileProvider();
        parser = new CardParser();
        saveService = new SaveService(outputPath);
        captureService = new CaptureService();
        pages = new List<PageView>();

        cardView.Initialize();

        folderPaths = GetFolderPaths(cardsPath);

        Debug.Log($"Path: {cardsPath}");
        Debug.Log($"Folders found: {folderPaths.Count()}");

        StartCapture();
    }

    private IEnumerable<string> GetFolderPaths(string path)
    {
        return System.IO.Directory.GetDirectories(path);
    }

    private IEnumerable<string> GetFilesInsideFolder(string folderPath)
    {
        return System.IO.Directory.GetFiles(folderPath, "*.md");
    }

    private string GetFolderName(string folderPath)
    {
        var parts = folderPath.Split(System.IO.Path.DirectorySeparatorChar);
        var folderName = parts[parts.Length - 1];

        return folderName;
    }

    public void StartCapture()
    {
        Capture();
    }

    private async void Capture()
    {
        pages.Clear();

        foreach (var folderPath in folderPaths)
        {
            var folderName = GetFolderName(folderPath);
            var filesInFolder = GetFilesInsideFolder(folderPath);

            CreateNewPage();

            Debug.Log($"Loading files from path {folderPath}. Found {filesInFolder.Count()} files.");

            foreach (var filePath in filesInFolder)
            {
                string text = await _textProvider.LoadText(filePath);

                saveService.PrepareFolder(folderName);

                var cards = parser.SplitIntoSeparateCards(text);

                foreach (var card in cards)
                {
                    HideCard(cardView);
                    await ShowCard(card);

                    var front = await captureService.CaptureCardFront(cardView);
                    var back = await captureService.CaptureCardBack(cardView);

                    saveService.SaveTexture(front, folderName);
                    saveService.SaveTexture(back, folderName);

                    AddToCurrentPage(front, back);

                    if (IsCurrentPageFull())
                    {
                        await SaveCurrentPage(folderName);

                        CreateNewPage();
                    }
                }
            }

            // If page was not full, save as it is
            if (IsCurrentPageEmpty() == false && IsCurrentPageFull() == false)
            {
                await SaveCurrentPage(folderName);
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

    private bool IsCurrentPageEmpty()
    {
        var page = GetCurrentPage();

        if (page == null) return true;

        return page.IsEmpty;
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

    private async Task ShowCard(string cardText)
    {
        CardController card = new CardController(cardText);

        card.Parse(parser);

        await cardView.Show(card.model);
    }

    private void HideCard(CardView cardView)
    {
        cardView.Hide();
    }
}