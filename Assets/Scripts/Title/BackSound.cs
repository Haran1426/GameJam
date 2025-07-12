using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackSound : MonoBehaviour
{
    [SerializeField] Slider musicslider;
    [SerializeField] AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        musicslider.value = PlayerPrefs.GetFloat("backsound", 1f);
        audio.volume = musicslider.value;
        musicslider.onValueChanged.AddListener(delegate { Update(); });
    }

    // Update is called once per frame
    void Update()
    {
        audio.volume = musicslider.value;
        PlayerPrefs.SetFloat("backsound", musicslider.value);
        PlayerPrefs.Save();
    }
}
