using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class LanguagePair : ScriptableObject
{
    // shortname of source and target language
    public string languagepair_shortname; // e.g. "en_zh"
    public string source_shortname; // e.g. "en"
    public string target_shortname; // e.g. "zh"

    // two_dimensional: true for logographic, false for phonetic languages
    // (where the phonetic script of the logographice language represents the second dimension)
    public bool source_two_dimensional; // i.c.o. "en" --> False
    public bool target_two_dimensional; // i.c.o. "zh" --> True

    // path to data files
    public string dictionary_file; // frequency ordered dictionary (for speed reasons) --> path to file
    public string target_text_corpus; // large text corpus in target language for exercises --> path to file
    public string vocab_file; // file where the vocab and user's progress on it is stored
    public string practice_texts_folder; // folder with subfolders from 1 to 5, each with practice texts in them

    // get a LanguagePair scriptable object from string
    public static LanguagePair get_languagepair_from_string(string s)
    {
        string path_to_languagepair_folder = "Assets/Scripts/Subclasses/LanguagePairs/" + s + ".asset";
        return AssetDatabase.LoadAssetAtPath<LanguagePair>(path_to_languagepair_folder);
    }
}
