using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] Slider musicVolumn;

    // Start is called before the first frame update
    public void Start()
    {
        Load();
    }

    public void ChangeVolunm()
    {
        AudioListener.volume = musicVolumn.value;
        Save();
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("musicVolumn"))
        {
            PlayerPrefs.SetFloat("musicVolumn", musicVolumn.maxValue);
        }
        Debug.Log(PlayerPrefs.HasKey("musicVolumn"));
        musicVolumn.value = PlayerPrefs.GetFloat("musicVolumn");
        
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolumn", musicVolumn.value);
    }

}
