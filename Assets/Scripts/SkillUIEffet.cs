using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SkillUIEffet : MonoBehaviour
{
    public Player player;

    public Image changeUI;
    public Image skillUI;

    void Start()
    {
        
    }

    void Update()
    {
        if (player.changeDelay > 0f) changeUI.fillAmount = player.changeDelay / 2f;
        
        else changeUI.fillAmount = 0f;
        
        if(player.skillDelay > 0f) skillUI.fillAmount = player.skillDelay / 15f;

        else if(player.isSkill) skillUI.fillAmount = 1f;

        else skillUI.fillAmount = 0f;

    }
}
