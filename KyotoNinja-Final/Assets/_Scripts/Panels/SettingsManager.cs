using MyNavigationSystem;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backButton;

    [Header("Sliders")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Button Actions")]
    [SerializeField] private string settingsPanelId;
   
    private void Start()
    {
        backButton.onClick.AddListener(() => AudioManager.instance.PlayButtonSound());
        backButton.onClick.AddListener(() => NavigationManager.Instance.HidePopUp(settingsPanelId));
    }

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        AudioManager.instance.sfxVolume = sfxSlider.value;
        AudioManager.instance.musicVolume = musicSlider.value;
    }
}