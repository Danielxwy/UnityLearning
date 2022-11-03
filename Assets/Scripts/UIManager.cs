using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PlayerController player;
    public Image healthBarUp;
    public Image healthBarDown;
    public Image energyBar;
    public float hpBarLerp = 3.0f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        healthBarDown.fillAmount = Mathf.Lerp(healthBarDown.fillAmount, player.CurrentHealth / player.maxHealth, hpBarLerp * Time.deltaTime);
        healthBarUp.fillAmount = player.CurrentHealth / player.maxHealth;
        energyBar.fillAmount -= 1.0f / player.dashCD * Time.deltaTime;
    }

    public void InitEnergyBar()
    {
        energyBar.fillAmount = 1;
    }
}

