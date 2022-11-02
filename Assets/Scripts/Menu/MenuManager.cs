using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    private float highScoreLevel1;
    private float highScoreLevel2;

    private void Start()
    {
        highScoreLevel1 = PlayerPrefs.GetInt("HighScore" + Constants.LEVEL1_SCENE_NAME,0);        
        highScoreLevel2 = PlayerPrefs.GetInt("HighScore" + Constants.LEVEL2_SCENE_NAME,0);
        
        if(highScoreLevel1 < Constants.POINTS_UNLOCK_LEVEL2)
        {
            level2Button.interactable = false;
        }
        else
        {
            level2Button.interactable = true;
        }

        if (highScoreLevel2 < Constants.POINTS_UNLOCK_LEVEL3)
        {
            level3Button.interactable = false;
        }
        else
        {
            level3Button.interactable = true;
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }
}
