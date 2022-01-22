using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
