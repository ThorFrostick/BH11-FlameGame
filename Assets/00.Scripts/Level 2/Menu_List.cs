using System.Collections;
using System.Collections.Generic;
using _00.Scripts.Manager;
using UnityEngine;

public class Menu_List : MonoBehaviour, IResetable
{
    public void Reset()
    {
        this.gameObject.SetActive(false);
    }
    
}
