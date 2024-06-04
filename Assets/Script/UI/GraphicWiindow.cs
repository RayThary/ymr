using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicWiindow : MonoBehaviour
{
    FullScreenMode fullScreenMode;
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Toggle toggle;
    private int value;
    List<Resolution> resolutions = new ();
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value >= 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        dropdown.options.Clear();
        foreach (Resolution resolution in resolutions)
        {
            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = resolution.width + " x " + resolution.height + " " + resolution.refreshRateRatio.value + "Hz";
            dropdown.options.Add(optionData);

            if(resolution.width == Screen.width && resolution.height == Screen.height)
            {
                dropdown.value = value;
            }
            value++;
        }
        dropdown.RefreshShownValue();

        toggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropdownValueChange(int _value)
    {
        value = _value;
        Apply();
    }

    public void FullScreenBox(bool isFull)
    {
        fullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }

    private void Apply()
    {
        Screen.SetResolution(resolutions[value].width, resolutions[value].height, fullScreenMode);
        SoundManager.instance.SFXCreate(SoundManager.Clips.ButtonClip, 1, 0);
    }
}
