using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    Resolution[] resolutions;

    public Dropdown resolutionDropdown; 

    void Start(){
        resolutions =  Screen.resolutions;

        Screen.fullScreen = true;
        SetFullScreen(true);

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void SetFullScreen(bool setFull){
        Screen.fullScreen = setFull;
    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /*public void ChangeSceneToLoad(string sceneToLoadTxt){
        loadingSceneScript.sceneToLoad = sceneToLoadTxt;
    }*/
}