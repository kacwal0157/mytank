using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ButtonLanguageManager
{
    private PlayerMetadataManager playerMetadataManager;
    private PublicGameObjects publicGameObjects;
    private GameObject activeLanguageButton;
    private string activeLanguage;
    private Dictionary<string, string> languageDictionary;
    private Dictionary<string, Image> languageDictionaryImage;
    private GameObject settingsLanguageIconGameObject;
    private TextMeshProUGUI settingIconText;
    private Image settingIconImage;

    public ButtonLanguageManager(PlayerMetadataManager playerMetadataManager, PublicGameObjects publicGameObjects)
    {
        this.playerMetadataManager = playerMetadataManager;
        this.publicGameObjects = publicGameObjects;

        activeLanguage = playerMetadataManager.language;
        settingsLanguageIconGameObject = publicGameObjects.settingIconLanguageGameObject;
        languageDictionary = dictionaryOfLanguages();
        languageDictionaryImage = dictionaryOfLanguagesImage();

        settingIconText = settingsLanguageIconGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        settingIconImage = settingsLanguageIconGameObject.transform.GetChild(1).gameObject.GetComponent<Image>();

        List<Button> buttons = publicGameObjects.buttonLanguageGroup.GetComponentsInChildren<Button>().ToList();
        foreach(Button b in buttons)
        {
            b.onClick.AddListener(()=> onClick(b.gameObject));
            if(b.name.Equals("Flag_" + activeLanguage))
            {
                b.transform.GetChild(0).gameObject.SetActive(true);
                activeLanguageButton = b.gameObject;
                setTextAndImageOfSettingLanguage();
            }
        }
    }

    private void onClick(GameObject go)
    {
        activeLanguageButton.transform.GetChild(0).gameObject.SetActive(false);
        go.transform.GetChild(0).gameObject.SetActive(true);

        activeLanguageButton = go;
        activeLanguage = activeLanguageButton.name.Substring(5, 3);
        playerMetadataManager.language = activeLanguage;

        setTextAndImageOfSettingLanguage();
    }

    private void setTextAndImageOfSettingLanguage()
    {
        string value;
        Image valueImg;

        languageDictionary.TryGetValue(activeLanguage, out value);
        languageDictionaryImage.TryGetValue(activeLanguage, out valueImg);

        settingIconText.text = value;
        settingIconImage.sprite = valueImg.sprite;
    }

    private Dictionary<string, string> dictionaryOfLanguages()
    {
        var langDict = new Dictionary<string, string>();

        langDict.Add("ENG", "English");
        langDict.Add("KOR", "Korean");
        langDict.Add("CHN", "Chinese");
        langDict.Add("RUS", "Russian");
        langDict.Add("DEU", "Deutch");
        langDict.Add("TWN", "Taiwanese");
        langDict.Add("ESP", "Spanish");
        langDict.Add("JPN", "Japanese");
        langDict.Add("PRT", "Portuguese");
        langDict.Add("GRE", "Greek");
        langDict.Add("SWE", "Swedish");
        langDict.Add("UKR", "Ukrainian");
        langDict.Add("NED", "Dutch");
        langDict.Add("ITA", "Italian");
        langDict.Add("TUR", "Turkish");
        langDict.Add("POL", "Polski");
        langDict.Add("FRA", "French");
        langDict.Add("THA", "Thai");
        langDict.Add("ROU", "Romanian");
        langDict.Add("INA", "Indian");

        return langDict;
    }

    private Dictionary<string, Image> dictionaryOfLanguagesImage()
    {
        var langDictImg = new Dictionary<string, Image>();

        langDictImg.Add("ENG", publicGameObjects.settingIconImage[0]);
        langDictImg.Add("KOR", publicGameObjects.settingIconImage[1]);
        langDictImg.Add("CHN", publicGameObjects.settingIconImage[2]);
        langDictImg.Add("RUS", publicGameObjects.settingIconImage[3]);
        langDictImg.Add("DEU", publicGameObjects.settingIconImage[4]);
        langDictImg.Add("TWN", publicGameObjects.settingIconImage[5]);
        langDictImg.Add("ESP", publicGameObjects.settingIconImage[6]);
        langDictImg.Add("JPN", publicGameObjects.settingIconImage[7]);
        langDictImg.Add("PRT", publicGameObjects.settingIconImage[8]);
        langDictImg.Add("GRE", publicGameObjects.settingIconImage[9]);
        langDictImg.Add("SWE", publicGameObjects.settingIconImage[10]);
        langDictImg.Add("UKR", publicGameObjects.settingIconImage[11]);
        langDictImg.Add("NED", publicGameObjects.settingIconImage[12]);
        langDictImg.Add("ITA", publicGameObjects.settingIconImage[13]);
        langDictImg.Add("TUR", publicGameObjects.settingIconImage[14]);
        langDictImg.Add("POL", publicGameObjects.settingIconImage[15]);
        langDictImg.Add("FRA", publicGameObjects.settingIconImage[16]);
        langDictImg.Add("THA", publicGameObjects.settingIconImage[17]);
        langDictImg.Add("ROU", publicGameObjects.settingIconImage[18]);
        langDictImg.Add("INA", publicGameObjects.settingIconImage[19]);

        return langDictImg;
    }
}