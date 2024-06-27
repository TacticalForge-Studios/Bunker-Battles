using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public GameObject gunModel;

    [Range (1, 15)]public int shootDamage;
    [Range (0.1f, 10)]public float shootRate;
    [Range (15, 400)]public int shootDist;
    public int ammoCurrent;
    public int ammoMax;
    public int totalAmmoLeft;
    public int magCapacity;
    public int recoil;

    public ParticleSystem hitEffect;
    public AudioClip shootSound;
    [Range (0, 1)]public float shootVolume;

    public Image icon;
}
