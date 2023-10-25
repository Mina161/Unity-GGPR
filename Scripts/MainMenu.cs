using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    AudioSource title;

    [SerializeField]
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        if(AudioListener.volume == 0)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame ()
    {
        title.Stop();
        SceneManager.LoadScene(1);
    }

    public void ToggleMute()
    {
        if (toggle.isOn)
        {
            AudioListener.volume = 1;

        }
        else
        {
            AudioListener.volume = 0;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
