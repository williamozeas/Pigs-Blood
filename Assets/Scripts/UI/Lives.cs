using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public List<GameObject> lives;
    private int totalLives;

    public int remainingLives;

    public Sprite thereLife, goneLife;

    public void Start()
    {
        totalLives = lives.Count;
        remainingLives = totalLives;
    }

    public void LoseLife()
    {
        lives[totalLives-remainingLives].GetComponent<Image>().sprite = goneLife;
        remainingLives--;
        if (remainingLives == 0)
        {
            
        }
    }

    public void ResetLives()
    {
        remainingLives = totalLives;
        for (int i = 0; i < lives.Count; i++)
        {
            lives[i].GetComponent<Image>().sprite = thereLife;
        }
    }
}
