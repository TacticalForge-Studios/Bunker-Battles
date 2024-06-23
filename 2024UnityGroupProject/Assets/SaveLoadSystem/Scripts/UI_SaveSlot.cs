using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Events;

public class UI_SaveSlot : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI slotName;
    [SerializeField] TextMeshProUGUI lastSavedTime_Manual;
    [SerializeField] TextMeshProUGUI lastSavedTime_Automatic;

    [SerializeField] Image manualSaveBackground;
    [SerializeField] Image AutomaticSaveBackground;

    [SerializeField] Color defaultColor = Color.black;
    [SerializeField] Color selectedColor = Color.gray;

    public UnityEvent<ESaveSlot, ESaveType> OnSlotSelected = new UnityEvent<ESaveSlot, ESaveType>();
    
    
    
    UI_SaveLoadUI.EMode currentMode;
    ESaveSlot Slot;
    bool hasManualSave;
    bool hasAutomaticSave;

    public void PrepareForMode(UI_SaveLoadUI.EMode mode, ESaveSlot slot)
    {
        Slot = slot;
        currentMode = mode;

        hasManualSave = SaveLoadManager.instance.DoesSaveExist(slot, ESaveType.Manual);
        hasAutomaticSave = SaveLoadManager.instance.DoesSaveExist(slot, ESaveType.Automatic);

        slotName.text = $"Slot {(int)slot}";

        if (hasManualSave)
        {
            lastSavedTime_Manual.text = SaveLoadManager.instance.GetLastSavedTIme(slot, ESaveType.Manual);
        }
        else
        {
            lastSavedTime_Manual.text = currentMode == UI_SaveLoadUI.EMode.save ? "Empty" : "None";
        }

        if (hasAutomaticSave)
        {
            lastSavedTime_Automatic.text = SaveLoadManager.instance.GetLastSavedTIme(slot, ESaveType.Automatic);
        }
        else
        {
            lastSavedTime_Automatic.text = currentMode == UI_SaveLoadUI.EMode.save ? "Empty" : "None";
        }

        // in load mode hide empty slots
        if (currentMode == UI_SaveLoadUI.EMode.load && !hasManualSave && !hasAutomaticSave)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }

        if (currentMode == UI_SaveLoadUI.EMode.save)
        {
            AutomaticSaveBackground.gameObject.SetActive(false);
        }
        else
        {
            AutomaticSaveBackground.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        AutomaticSaveBackground.color = defaultColor;
        manualSaveBackground.color = defaultColor;
    }

    public void SetSelectedSlot(ESaveSlot slot)
    {
        if (slot != Slot)
        {
            AutomaticSaveBackground.color = defaultColor;
            manualSaveBackground.color = defaultColor;
        }
    }

    public void OnSelectManualSave()
    {
        if (!hasManualSave && currentMode == UI_SaveLoadUI.EMode.load)
        {
            return;
        }

        AutomaticSaveBackground.color = defaultColor;
        manualSaveBackground.color = selectedColor;

        OnSlotSelected.Invoke(Slot, ESaveType.Manual);
    }

    public void OnSelectAutomaticSave()
    {
        if (!hasAutomaticSave && currentMode == UI_SaveLoadUI.EMode.load)
        {
            return;
        }

        AutomaticSaveBackground.color = selectedColor;
        manualSaveBackground.color = defaultColor;

        OnSlotSelected.Invoke(Slot, ESaveType.Automatic);
    }


}
