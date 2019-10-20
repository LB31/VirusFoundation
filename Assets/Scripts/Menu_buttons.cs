using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_buttons : MonoBehaviour
{
   public void onExit()
   {
       Application.Quit();
   }
   public void onScene_Level1()
   {
        SceneManager.LoadScene("Main", LoadSceneMode.Additive);
    }
}
