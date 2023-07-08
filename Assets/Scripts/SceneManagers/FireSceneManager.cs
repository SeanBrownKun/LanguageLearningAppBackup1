using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FireSceneManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI EnergyPoints;
    [SerializeField] Image NewWordBorder;
    [SerializeField] Image MistakeBorder;
    [SerializeField] Image CorrectBorder;
    [SerializeField] TextMeshProUGUI IntroductionText;
    [SerializeField] TextMeshProUGUI MainText;
    [SerializeField] TextMeshProUGUI SubText;

    [SerializeField] Button Button1;
    [SerializeField] Button Button2;
    [SerializeField] Button Button3;
    [SerializeField] Button Button4;

    [SerializeField] TMP_InputField InputField;
    [SerializeField] Button OkButton;

    private string correct_answer;
    private bool made_mistake;
    private bool show_only_mode;

    private Color Enabled = new Color (r: 1, g: 1, b: 1, a: 1);
    private Color Disabled = new Color (r: 0, g: 0, b: 0, a: 0);

    void Start()
    {
        show_only_mode = false;
        MistakeBorder.color = Disabled;
        CorrectBorder.color = Disabled;
        UpdateLevelScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            clickButton(Button1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            clickButton(Button2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            clickButton(Button3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            clickButton(Button4);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            clickButton(OkButton);
        }
    }


    public void clickHomeButton()
    {
        Vocab.instance.SaveVocab();
        Game.instance.SaveGame();
        SceneManager.LoadScene(0);
    }

    public void clickButton(Button clicked_button)
    {
        
        if (show_only_mode)
        {   MistakeBorder.color = Disabled; CorrectBorder.color = Disabled;
            show_only_mode = false; Vocab.instance.get_next_Word(); UpdateLevelScreen(); return; }

        Word cw = Vocab.instance.current_word;
        if (cw.Level == 0) { cw.Level += 1; cw.Ranking += (int)System.Math.Pow((double)2, (double)cw.Level);
            UpdateLevelScreen();
        }
        else if (cw.Level > 0 && cw.Level < 6)
        {
            if (clicked_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == correct_answer)
            {
                // change level and ranking according to if a mistake was made or not
                if (made_mistake) { cw.Level -= 1; } else { cw.Level += 1; Game.instance.EnergyPoints += (cw.Level-1); }
                cw.Ranking += (int)System.Math.Pow((double)2, (double)cw.Level-1);
                show_only_mode = true;
                UpdateLevelScreen();
            } 
            else
            {
                made_mistake = true;
                if (cw.Level == 1) { IntroductionText.text = cw.SourceLang; } 
                else if (cw.Level == 2 && Settings.instance.current_languagepair.target_two_dimensional)
                    { SubText.text = cw.TargetPhonetic; }
                else if (cw.Level == 3 && Settings.instance.current_languagepair.target_two_dimensional)
                    { SubText.text = cw.TargetLang; }
                else if (cw.Level < 4) { SubText.text = cw.SourceLang; }
                else if (cw.Level == 4) { SubText.text = cw.TargetLang; }
                else if (cw.Level == 5 && Settings.instance.current_languagepair.target_two_dimensional)
                    { SubText.text = cw.TargetPhonetic; }
                else { SubText.text = cw.TargetLang; }
            }
        }
        else if (cw.Level > 5)
        {
            if (InputField.text == correct_answer)
            {
                // change level and ranking according to if a mistake was made or not
                if (made_mistake) { cw.Level -= 1; } else { cw.Level += 1; Game.instance.EnergyPoints += (cw.Level-1); }
                cw.Ranking += (int)System.Math.Pow((double)2, (double)cw.Level);
                show_only_mode = true;
                UpdateLevelScreen();
            }
            else
            {
                made_mistake = true;
                InputField.Select();
                SubText.text = cw.TargetLang;
            }
        }
        if (cw.Level > 1)
        {
            if (made_mistake) { MistakeBorder.color = Enabled; }
            else { if (MistakeBorder.color != Enabled) { CorrectBorder.color = Enabled; }}
        }
    }

    public void reset_screen()
    {
        NewWordBorder.color = new Color(r: 0, g: 0, b: 0, a: 0);
        IntroductionText.text = "";
        MainText.text = "";
        SubText.text = "";

        Button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; Button1.interactable = false;
        Button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; Button2.interactable = false;
        Button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; Button3.interactable = false;
        Button4.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; Button4.interactable = false;

        InputField.text = ""; InputField.interactable = false;
        OkButton.interactable = false;

        correct_answer = null;
        made_mistake = false;
    }

    private void SetButtonTexts(int type_idx, List<Word> fourwords) // type_idx: 0 Source, 1 Phonetic, 2 Target
    {
        if (type_idx == 0)
        {   Button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[0].SourceLang;
            Button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[1].SourceLang;
            Button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[2].SourceLang;
            Button4.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[3].SourceLang; }
        else if (type_idx == 1 && Settings.instance.current_languagepair.target_two_dimensional)
        {   Button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[0].TargetPhonetic;
            Button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[1].TargetPhonetic;
            Button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[2].TargetPhonetic;
            Button4.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[3].TargetPhonetic; }
        else if (type_idx == 2)
        {   Button1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[0].TargetLang;
            Button2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[1].TargetLang;
            Button3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[2].TargetLang;
            Button4.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fourwords[3].TargetLang; }
    }

    public void UpdateLevelScreen() // "show" means to show the word fully without a question
    {
        reset_screen();
        EnergyPoints.text = Game.instance.EnergyPoints.ToString();

        Word cw = Vocab.instance.current_word; // cw = current word
        LanguagePair clp = Settings.instance.current_languagepair; // clp = current language pair
        
        // level 0
        if (cw.Level == 0 || show_only_mode)
        {
            IntroductionText.text = cw.SourceLang;
            MainText.text = cw.TargetLang;
            if (clp.target_two_dimensional) { SubText.text = cw.TargetPhonetic; }
            OkButton.interactable = true;
            if (cw.Level == 0) { NewWordBorder.color = Enabled; }
        }
        // level 1-5
        else if (cw.Level > 0 && cw.Level < 6){
            List<Word> four_words = Vocab.instance.get_four_words(cw);
            Button1.interactable = true; Button2.interactable = true;
            Button3.interactable = true; Button4.interactable = true;

            if (clp.target_two_dimensional)
            {
                if (cw.Level == 1)
                {
                    MainText.text = cw.TargetLang;
                    SubText.text = cw.TargetPhonetic;
                    SetButtonTexts(0, four_words);
                    correct_answer = cw.SourceLang;
                    Debug.Log(cw.TargetLang);
                }
                else if (cw.Level == 2)
                {
                    MainText.text = cw.TargetLang;
                    SetButtonTexts(1, four_words);
                    correct_answer = cw.TargetPhonetic;
                }
                else if (cw.Level == 3)
                {
                    MainText.text = cw.TargetPhonetic;
                    SetButtonTexts(2, four_words);
                    correct_answer = cw.TargetLang;
                }
                else if (cw.Level == 4)
                {
                    MainText.text = cw.SourceLang;
                    SetButtonTexts(2, four_words);
                    correct_answer = cw.TargetLang;
                }
                else if (cw.Level == 5)
                {
                    MainText.text = cw.SourceLang;
                    SetButtonTexts(1, four_words);
                    correct_answer = cw.TargetPhonetic;
                }
            }
            else
            {
                if (cw.Level < 4)
                {
                    MainText.text = cw.TargetLang;
                    SetButtonTexts(0, four_words);
                    correct_answer = cw.SourceLang;
                }
                else
                {
                    MainText.text = cw.SourceLang;
                    SetButtonTexts(2, four_words);
                    correct_answer = cw.TargetLang;
                }
            }
        }
        // level 6
        else
        {
            MainText.text = cw.SourceLang;
            InputField.interactable = true;
            InputField.Select();
            OkButton.interactable = true;
            correct_answer = cw.TargetLang;
        }
    }
}
