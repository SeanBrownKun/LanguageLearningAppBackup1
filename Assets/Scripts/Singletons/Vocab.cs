using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vocab : MonoBehaviour
{
    public static Vocab instance = null;
    public List<Word> vocab = new List<Word>();
    public List<Word> active_vocab = new List<Word>();
    public Word current_word;
    public int autosave_counter_count;


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

    // loads the current word on startup
    public void load_current_word()
    {
        Word cw = null;
        foreach (Word word in active_vocab)
        {
            if ((cw == null) || (word.Ranking < cw.Ranking)) { cw = word; }
        }
        current_word = cw;
    }
    // gets the next word to learn and diminishes all other rankings
    public void get_next_Word()
    {
        Word next_word = null;
        int add_inactive_score = 0;

        foreach (Word word in active_vocab)
        {
            // reduce ranking
            word.Ranking -= 1;
            if ((next_word == null) || (word.Ranking < next_word.Ranking))
            {
                next_word = word;
            }
            // increase "add inactive word" score
            if (word.Level < 7) { add_inactive_score += (7 - word.Level); }
        }
        current_word = next_word;

        if (add_inactive_score < 100)
        {
            foreach (Word word in vocab)
            {
                if (!word.Active)
                {
                    word.Active = true;
                    word.Ranking = current_word.Ranking - 1;
                    active_vocab.Add(word);
                    break;
                }
            }
        }

        autosave_counter_count += 1;
        if (autosave_counter_count > Settings.instance.autosave_counter)
        {
            SaveVocab();
            Game.instance.SaveGame();
            autosave_counter_count = 0;
        }
    }

    // gets four random words from active vocab for the four buttons
    public List<Word> get_four_words(Word correct_word)
    {
        int correct_idx = active_vocab.IndexOf(correct_word);
        int idx1 = Random.Range(0, active_vocab.Count);
        while (idx1 == correct_idx)
        {
            idx1 = Random.Range(0, active_vocab.Count);
        }
        int idx2 = Random.Range(0, active_vocab.Count);
        while ((idx1 == idx2) || (idx2 == correct_idx))
        {
            idx2 = Random.Range(0, active_vocab.Count);
        }
        int idx3 = Random.Range(0, active_vocab.Count);
        while ((idx1 == idx3) || (idx2 == idx3) || (idx3 == correct_idx))
        {
            idx3 = Random.Range(0, active_vocab.Count);
        }

        int[] idxs = new int[] { idx1, idx2, idx3, correct_idx };
        List<int> ret_idxs = new List<int>();
        List<Word> ret_list = new List<Word>();

        while (ret_idxs.Count < 4)
        {
            bool inside = false;
            int rand_idx = Random.Range(0, 4);
            foreach (int idx in ret_idxs)
            {
                if (idxs[rand_idx] == idx)
                {
                    inside = true;
                    break;
                }
            }
            if (!inside)
            {
                ret_idxs.Add(idxs[rand_idx]);
            }
        }
        foreach (int idx in ret_idxs)
        {
             ret_list.Add(active_vocab[idx]);
        }
        return ret_list;
    }

    public void LoadVocab()
    {
        // clear vocab
        vocab.Clear();
        active_vocab.Clear();
        // read file
        StreamReader strReader = new StreamReader(Settings.instance.current_languagepair.vocab_file);
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
            Word temp_word;
            // load the words, discriminating between one- and two-dimensional-target-language vocab file
            if (Settings.instance.current_languagepair.target_two_dimensional)
            {
                temp_word = new Word(source: info[0], phonetic: info[1], target: info[2], 
                                          level: info[3], ranking: info[4], active: info[5]);
            }
            else
            {
                temp_word = new Word(source: info[0], target: info[1],
                                          level: info[2], ranking: info[3], active: info[4]);
            }
            Vocab.instance.vocab.Add(temp_word);
            if (temp_word.Active) { Vocab.instance.active_vocab.Add(temp_word); }
        }
        autosave_counter_count = 0;
    }
    
    public void SaveVocab(bool reset = false)
    {
        int reset_activation_counter = 0;
        using (StreamWriter strWriter = new StreamWriter(Settings.instance.current_languagepair.vocab_file))
        {
            foreach (var word in Vocab.instance.vocab)
            {
                // save word
                List<string> line = new List<string>();
                line.Add(word.SourceLang);
                if (Settings.instance.current_languagepair.target_two_dimensional)
                {
                    line.Add(word.TargetPhonetic);
                }
                line.Add(word.TargetLang);
                // save score
                if (reset)
                {
                    line.Add("0"); line.Add("0");
                    if (reset_activation_counter < 10) { line.Add("1"); } else { line.Add("0"); }
                    reset_activation_counter += 1;
                }
                else
                {
                    line.Add(word.Level.ToString());
                    line.Add(word.Ranking.ToString());
                    if (word.Active) { line.Add("1"); } else { line.Add("0"); }
                }
                strWriter.WriteLine(string.Join(";", line));
            }
        }
    }
}

