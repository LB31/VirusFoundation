using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_buttons : MonoBehaviour
{
   public void onExit()
   {
       Application.Quit();
   }
   public void onScene_Level1()
   {
       Application.LoadLevel(1);
   }
}
