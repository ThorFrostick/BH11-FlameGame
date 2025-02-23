using System.Collections;
using System.Collections.Generic;
using _01.Levels.Level_5;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOpen : Custom_Button
{
    public override void OnClicked()
    {
        SceneManager.LoadScene("Level 1");

    }
}
