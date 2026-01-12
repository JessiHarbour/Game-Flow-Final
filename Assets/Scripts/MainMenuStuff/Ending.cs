using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Ending : MonoBehaviour
{
      public void End()

   {
      SceneManager.LoadScene("MainMenu");
   }
}
