using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        highscoreEntryList = new List<HighscoreEntry>();

        // Load previously saved high scores
        LoadHighscores();

        // Sort and display high scores
        highscoreEntryList.Sort((x, y) => y.waveCount.CompareTo(x.waveCount));
        highscoreEntryTransformList = new List<Transform>();
        for (int i = 0; i < highscoreEntryList.Count && i < 10; i++)
        {
            CreateHighscoreEntryTransform(highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 20f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        entryTransform.Find("scoreText").GetComponent<Text>().text = highscoreEntry.waveCount.ToString();
        entryTransform.Find("nameText").GetComponent<Text>().text = highscoreEntry.name;

        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int waveCount, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { waveCount = waveCount, name = name };
        highscoreEntryList.Add(highscoreEntry);
        SaveHighscores();

        // Reload highscore table
        highscoreEntryList.Sort((x, y) => y.waveCount.CompareTo(x.waveCount));
        highscoreEntryTransformList = new List<Transform>();
        foreach (Transform child in entryContainer)
        {
            if (child == entryTemplate) continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < highscoreEntryList.Count && i < 10; i++)
        {
            CreateHighscoreEntryTransform(highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        }
    }

    private void SaveHighscores()
    {
        PlayerPrefs.SetString("highscoreTable", JsonUtility.ToJson(new Highscores { highscoreEntryList = highscoreEntryList }));
        PlayerPrefs.Save();
    }

    private void LoadHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable", JsonUtility.ToJson(new Highscores { highscoreEntryList = new List<HighscoreEntry>() }));
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        highscoreEntryList = highscores.highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int waveCount;
        public string name;
    }

    [System.Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
}
