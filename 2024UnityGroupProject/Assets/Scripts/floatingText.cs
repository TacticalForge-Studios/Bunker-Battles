using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class floatingText : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text expText;
    [SerializeField] TMP_Text expNumText;

    float timeAlive;
    public static int damage;
    public static int exp;
    [SerializeField] bool isDamage;
    [SerializeField] bool isExp;

    private void Start()
    {
        if (isDamage)
        {
            text.text = damage.ToString("F0");
        }
        else if(isExp)
        {
            expNumText.text = exp.ToString("F0");
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        transform.position += new Vector3(0, 1f, 0) * Time.deltaTime;

        if(timeAlive >= 1)
        {
            Destroy(gameObject);
        }
    }


}
