using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    public string SourceLang;
    public string TargetPhonetic;
    public string TargetLang;

    public int Level;
    public int Ranking;
    public bool Active;
    public Word(string source = null, string phonetic = null, string target = null,  string level = "0", string ranking = "0", string active = "0")
    {
        SourceLang = source;
        TargetPhonetic = phonetic;
        TargetLang = target;

        Level = Convert.ToInt32(level);
        Ranking = Convert.ToInt32(ranking);
        if (Convert.ToInt32(active) == 0) { Active = false; } else { Active = true; };
        Active = Convert.ToBoolean(Active);
    }
}
