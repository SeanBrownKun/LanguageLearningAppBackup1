using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance = null;

    public LanguagePair current_languagepair;
    public int add_inactive_word_threshold;
    public int autosave_counter;

    public bool first_time_startup = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void LoadSettings()
    {
        using (StreamReader strReader = new StreamReader("Assets/Data/Settings.txt"))
        {
            bool endOfFile = false;
            while (!endOfFile)
            {
                string data_String = strReader.ReadLine();
                if (data_String == null)
                {
                    endOfFile = true;
                    break;
                }
                var info = data_String.Split(';');
                current_languagepair = LanguagePair.get_languagepair_from_string(info[0]);
                add_inactive_word_threshold = Convert.ToInt32(info[1]);
                autosave_counter = Convert.ToInt32(info[2]);
            }
        }
        first_time_startup = false;
    }

    public void SaveSettings()
    {
        using (StreamWriter strWriter = new StreamWriter("Assets/Data/Settings.txt"))
        {
            List<string> line = new List<string>();
            line.Add(current_languagepair.languagepair_shortname);
            line.Add(add_inactive_word_threshold.ToString());
            line.Add(autosave_counter.ToString());
            
            strWriter.WriteLine(string.Join(";", line));
        }
    }
 
}
