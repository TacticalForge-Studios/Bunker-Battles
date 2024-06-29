using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UI_SaveLoadUI : MonoBehaviour
{
    [SerializeField] EMode mode;
    [SerializeField] Transform slotUIRoot;
    [SerializeField] GameObject slotUIPrefab;

    [SerializeField] GameObject SaveLoadButton;
    [SerializeField] TextMeshProUGUI saveLoadButtonText;

    [SerializeField] UnityEvent onLevelLoadReady = new UnityEvent();

    List<UI_SaveSlot> AllSlots = new List<UI_SaveSlot>();
    
    ESaveSlot selectedSlot;
    ESaveType selectedType;

    public enum EMode
    {
        save,
        load
    }

    private void OnEnable()
    {
        SaveLoadButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshUI();
    }
    void RefreshUI()
    {
        saveLoadButtonText.text = mode == EMode.load ? "Load Saved Game" : "Save Game";
        foreach (var slotUI in AllSlots)
        {
            Destroy(slotUI.gameObject);
        }

        AllSlots.Clear();

        var allSlots = System.Enum.GetValues(typeof(ESaveSlot));
        foreach (var slot in allSlots)
        {
            var slotEnum = (ESaveSlot)slot;

            if (slotEnum == ESaveSlot.None)
                continue;

            var slotUIGO = Instantiate(slotUIPrefab, slotUIRoot);
            var slotUI = slotUIGO.GetComponent<UI_SaveSlot>();

            slotUI.PrepareForMode(mode, slotEnum);
            slotUI.OnSlotSelected.AddListener(OnSlotSelected);

            AllSlots.Add(slotUI);


        }

        selectedSlot = ESaveSlot.None;

        SaveLoadButton.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnSlotSelected(ESaveSlot slot, ESaveType saveType)
    {
        selectedSlot = slot;
        selectedType = mode == EMode.save ? ESaveType.Manual : saveType;
        SaveLoadButton.SetActive(true);

        foreach (var slotUI in AllSlots)
        {
            slotUI.SetSelectedSlot(slot);
        }
    }

    public void OnPerformSaveLoad()
    {
        if (mode == EMode.load)
        {
            SaveLoadManager.instance.RequestLoad(selectedSlot, selectedType);
            onLevelLoadReady.Invoke();
        }
        else
        {
            SaveLoadManager.instance.RequestSave(selectedSlot, selectedType);
            RefreshUI();
        }
       
    }

    public void SetMode_Save()
    {
        if (mode == EMode.save)
        {
            return;
        }

        mode = EMode.save;
        RefreshUI();

    }

    public void SetMode_Load()
    {
        if (mode == EMode.load)
            return;

        mode = EMode.load;
        RefreshUI();
    }
}
