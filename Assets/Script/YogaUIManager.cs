using Core3lb;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class YogaUIManager : MonoBehaviour
{
    public GameObject breathExersizeObject;
    public GameObject eyeExersizeObject;
    public GameObject meditationObject;
    public GameObject chairYogaObject;
    public GameObject headRotationObject;
    public GameObject anulomVilomObject;
    public GameObject themeUI;
    public GameObject menuUI;
    public Canvas yogaCanvas;
    public GameObject yogaTrainingUI;
    public GameObject audioMenuUI;
    public GameObject helpUI;
    public GameObject whisperOfTheDayUI;
    public GameObject[] screens;
    public GameObject[] allExersizes;

    public Image quoteBG;
    public TextMeshProUGUI quoteText;
    public string QuoteURL;
    public string jsonFile;
    public QuoteClass quoteClass = new QuoteClass();
    void Start()
    {
        GetQuoteFromAPI();
    }

    public void GetQuoteFromAPI()
    {
        StartCoroutine(IEGetQuoteFromAPI());
    }

    public IEnumerator IEGetQuoteFromAPI()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(QuoteURL))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.LogError("Error :" + www.error);
            }
            else
            {
                jsonFile = www.downloadHandler.text;
                quoteClass = JsonUtility.FromJson<QuoteClass>(jsonFile);
                quoteText.text = quoteClass.quote.ToString();
                StartCoroutine(LoadImage(quoteClass.backgroundURL, quoteBG));
            }
        }
    }

    IEnumerator LoadImage(string url, Image img)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading image: " + webRequest.error);
            }
            else
            {
                Texture2D texture = UnityEngine.Networking.DownloadHandlerTexture.GetContent(webRequest);

                Sprite newSprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f));

                img.sprite = newSprite;
            }
        }
    }

    public void ConvertIntoJson()
    {
        jsonFile = JsonUtility.ToJson(quoteClass);
    }

    public void HandleScreens(GameObject screen = null)
    {
        foreach (GameObject go in screens)
        {
            go.SetActive(false);
        }
        if (screen != null)
        {
            screen.SetActive(true);
        }
    }

    public void OnClickBackButton()
    {
        HandleScreens(menuUI);
    }
    public void DisableAllExersizes()
    {
        foreach (var exer in allExersizes)
        {
            exer.gameObject.SetActive(false);
        }
        yogaCanvas.gameObject.SetActive(false);
    }
    public void DoBreatheExersize()
    {
        HandleScreens();
        DisableAllExersizes();
        breathExersizeObject.SetActive(true);
    }

    public void DoEyeExersizeObject()
    {
        HandleScreens();
        DisableAllExersizes();
        eyeExersizeObject.SetActive(true);
    }
    public void DoAnulomVilomExersize()
    {
        HandleScreens();
        DisableAllExersizes();
        anulomVilomObject.SetActive(true);
    }
    public void DoMeditation()
    {
        HandleScreens();
        DisableAllExersizes();
        meditationObject.SetActive(true);
    }
    public void DoChairYoga()
    {
        HandleScreens();
        DisableAllExersizes();
        chairYogaObject.SetActive(true);
    }
    public void DoNeckRotationYoga()
    {
        HandleScreens();
        DisableAllExersizes();
        headRotationObject.SetActive(true);
    }
    public void HandleThemeUI()
    {
        HandleScreens(themeUI);
    }

    public void OnClickMenuButton()
    {
        DisableAllExersizes();
        if (!yogaCanvas.gameObject.activeInHierarchy)
        {
            yogaCanvas.gameObject.SetActive(true);
            HandleScreens(menuUI);
        }
        else
        {
            yogaCanvas.gameObject.SetActive(false);
        }
    }
    public void HandleyogaTrainingUI()
    {
        HandleScreens(yogaTrainingUI);
    }
    public void HandlewhisperOfTheDayUI()
    {
        HandleScreens(whisperOfTheDayUI);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HandleAudioUI()
    {
        if (audioMenuUI.activeInHierarchy)
        {
            audioMenuUI.SetActive(false);
        }
        else
        {
            HandleScreens(audioMenuUI);
        }
    }
    public void HandleHelpUI()
    {
        if (helpUI.activeInHierarchy)
        {
            helpUI.SetActive(false);
        }
        else
        {
            HandleScreens(helpUI);
        }
    }
}

[System.Serializable]
public class QuoteClass
{
    public string quote;
    public string backgroundURL;
}