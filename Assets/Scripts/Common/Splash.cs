using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private void Start(){
        SceneManager.LoadSceneAsync("BoardScene");
    }
}
