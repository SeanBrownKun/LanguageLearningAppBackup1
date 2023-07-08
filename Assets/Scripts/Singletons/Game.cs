using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance = null;

    public int EnergyPoints;

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

    public void LoadGame()
    {
        using (StreamReader strReader = new StreamReader("Assets/Data/Game.txt"))
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
                EnergyPoints = Convert.ToInt32(info[0]);
            }
        }
    }

    public void SaveGame()
    {
        using (StreamWriter strWriter = new StreamWriter("Assets/Data/Game.txt"))
        {
            List<string> line = new List<string>();
            line.Add(EnergyPoints.ToString());
            line.Add("meiyouyisi");

            strWriter.WriteLine(string.Join(";", line));
        }
    }

}
