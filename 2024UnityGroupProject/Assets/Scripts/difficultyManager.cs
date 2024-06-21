using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class difficultyManager : MonoBehaviour
{
    static bool easy;
    static bool medium;
    static bool hard;

    [SerializeField] bullet gunnerBullet;
    [SerializeField] bullet sniperBullet;
    [SerializeField] bullet generalBullet;
    [SerializeField] meleeHit meleeDamage;

    public void setDifficultyEasy()
    {
        medium = false;
        hard = false;

        if(! easy)
        {
            gunnerBullet.setDamage(1);
            sniperBullet.setDamage(4);
            generalBullet.setDamage(3);
            meleeDamage.setDamage(3);
            easy = true;
        }
        
    }

    public void setDifficultyMedium()
    {
        easy = false;
        hard = false;
        if (!medium)
        {
            gunnerBullet.setDamage(2);
            sniperBullet.setDamage(8);
            generalBullet.setDamage(6);
            meleeDamage.setDamage(5);
            medium = true;
        }
    }

    public void setDifficultyHard()
    {
        easy = false;
        medium = false;
        if (!hard)
        {
            gunnerBullet.setDamage(3);
            sniperBullet.setDamage(12);
            generalBullet.setDamage(9);
            meleeDamage.setDamage(8);
            hard = true;
        }
    }

    static public int getMeleeDamage()
    {
        if (easy)
        {
            return 3;
        }
        else if(medium)
        {
            return 5;
        }
        else if (hard)
        {
            return 8;
        }
        return 0;
    }
    
}
