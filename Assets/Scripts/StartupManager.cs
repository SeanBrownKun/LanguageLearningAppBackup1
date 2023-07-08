using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupManager : MonoBehaviour
{
    void Start()
    {
        // load the game elements if the game has just been started up
        if (Settings.instance.first_time_startup){
            Settings.instance.LoadSettings();
            Game.instance.LoadGame();
            Vocab.instance.LoadVocab(); Vocab.instance.load_current_word();
        }
    }

}
