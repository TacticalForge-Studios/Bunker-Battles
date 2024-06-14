using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sens;
    [SerializeField] int lovkVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] float recoil;

    float rotX;
    float recoilTimer = 0;
    
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

        if (Input.GetButton("Fire1") && gameManager.instance.playerScript.checkGunList() && gameManager.instance.playerScript.getIsShooting() && recoilTimer >= gameManager.instance.playerScript.getShootRate())
        {
            rotX -= recoil;
            recoilTimer = 0;
        }

        if (recoilTimer < gameManager.instance.playerScript.getShootRate())
        {
            recoilTimer += Time.deltaTime;
        }
    }
}
