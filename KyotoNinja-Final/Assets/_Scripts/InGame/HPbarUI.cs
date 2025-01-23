using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbarUI : MonoBehaviour
{
    public PlayerHP playerHP;
    public Sprite state1;
    public Sprite state2;
    public Sprite state3;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (playerHP != null)
        {
            if (playerHP.health == 1)
            {
                image.sprite = state1;
            }
            else if (playerHP.health == 2)
            {
                image.sprite = state2;
            }
            else
            {
                image.sprite = state3;
            }
        }
    }
}
