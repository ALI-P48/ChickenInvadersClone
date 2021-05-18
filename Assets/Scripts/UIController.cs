using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Sprite heart_on;
    public Sprite heart_off;
    public Image[] hearts;

    public GameObject menu;
    public GameObject ingame;

    void Start()
    {
        ChangeState("Menu");
    }

    public void UpdateHearts(float health)
    {
        int k = Mathf.FloorToInt(health) / 20;
        for (int i = 0; i < k; i++) {
            hearts[i].sprite = heart_on;
        }
        if (k < 5) {
            for (int i = k; i < 5; i++) {
                hearts[i].sprite = heart_off;
            }
        }
    }

    public void ChangeState(string newPage)
    {
        switch (newPage) {
            case "InGame":
                menu.SetActive(false);
                ingame.SetActive(true);
                UpdateHearts(100f);
                break;
            case "Menu":
                menu.SetActive(true);
                ingame.SetActive(false);
                break;
            default:
                menu.SetActive(false);
                ingame.SetActive(false);
                break;
        }
    }
}
