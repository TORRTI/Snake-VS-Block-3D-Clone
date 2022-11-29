using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public void NextLevel(int _sceneNumber)
    {
        SceneManager.LoadScene(_sceneNumber);
    }
}
