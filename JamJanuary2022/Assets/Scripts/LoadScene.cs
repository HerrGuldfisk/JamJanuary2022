using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string scenePath;

    public void InitiateLoad(){
        SceneManager.LoadScene(scenePath);
    }
}
