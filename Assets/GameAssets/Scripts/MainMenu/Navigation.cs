using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject cameraPos1;
    public GameObject cameraPos2;

    public void ChangeScene(string sceneName)       //Used for the navigation between scenes
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()      //Used to quit the application
    {
        Application.Quit();
    }

    public void CameraMoveToOptions()
    {
        Camera.main.transform.position = cameraPos2.transform.position;
        Camera.main.transform.rotation = cameraPos2.transform.rotation;
    }
    public void CameraMoveToMenu()
    {
        Camera.main.transform.position = cameraPos1.transform.position;
        Camera.main.transform.rotation = cameraPos1.transform.rotation;
    }
}

