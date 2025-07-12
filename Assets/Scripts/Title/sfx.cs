using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sfx : MonoBehaviour
{
    [SerializeField] Slider audioslider;
    [SerializeField] public AudioSource main;
    // Start is called before the first frame update
    void Start()
    {
        audioslider.value = PlayerPrefs.GetFloat("mainAudioSource", 0.6f);
        main.volume = audioslider.value; //효과음 volume를 슬라이더 value로 저장
        audioslider.onValueChanged.AddListener(delegate { SetVolume(); });
    }

    public void SetVolume()
    {
        main.volume = audioslider.value;
        PlayerPrefs.SetFloat("mainAudioSource", audioslider.value); // 볼륨 값 저장
        PlayerPrefs.Save();
    }
}
