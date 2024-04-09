using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLevelSelect : MonoBehaviour
{
    private int levelNumber;
    public int LevelNumber
    {
        get { return levelNumber; }
        set 
        { 
            levelNumber = value;
            string levelName = levelNumber + " Level ";
            name = levelName;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
        }
    }

    private void Start()
    {
        string levelName = levelNumber + " Level ";
        name = levelName;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    public void LoadSceneLevel()
    {
        SceneManager.LoadScene("Level" + levelNumber);
    }
}
