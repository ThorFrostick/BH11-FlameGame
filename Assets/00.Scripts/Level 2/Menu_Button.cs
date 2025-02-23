using System.Collections;
using System.Collections.Generic;
using _01.Levels.Level_5;
using UnityEngine;

public class Menu_Button : Custom_Button
{
    public override void OnClicked()
    {
        foreach (GameObject menu in m_affectedObjects)
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);  
            }
            else
            {
                menu.SetActive(true);
            }
        }
        
    }
}
