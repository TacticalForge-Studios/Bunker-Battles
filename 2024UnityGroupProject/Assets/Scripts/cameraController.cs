using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lovkVertMin, lockVertMax;
    [SerializeField] bool invertY;


    float rotX;
    float recoilTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        rotX = Mathf.Clamp(rotX, lovkVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

        recoilTimer += Time.deltaTime;

        if (Input.GetButton("Fire1") && gameManager.instance.playerScript.checkGunList() && gameManager.instance.playerScript.getIsShooting() && recoilTimer >= gameManager.instance.playerScript.getShootRate())
        {
            rotX -= gameManager.instance.playerScript.getRecoil();
            recoilTimer = 0;
        }

        if (gameManager.instance.playerScript.getDidGunChange())
        {
            recoilTimer += gameManager.instance.playerScript.getShootRate();
        }
    }
}
