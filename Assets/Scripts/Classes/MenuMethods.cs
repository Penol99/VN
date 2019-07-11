using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuMethods : MonoBehaviour {

    public List<Text> slots = new List<Text>();
    


    public void NewGame()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("scene_game");
    }

    public void SlotAccess(int slot)
    {

        if (PlayerPrefs.GetString("SlotType") == "Load")
        {
            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Saves" + "/Slot" + slot + ".txt"))
            {
                PlayerPrefs.SetInt("SlotLoaded", slot);
                PlayerPrefs.Save();
                SceneManager.LoadScene("scene_game");
            }
        }
        if (PlayerPrefs.GetString("SlotType") == "Save")
        {
            SaveAndLoad save = GetComponent<SaveAndLoad>();
            SetSlotText(slots, slot);
            save.SaveGame(slot);
        }

    }

    public void SaveButton()
    {
        PlayerPrefs.SetString("SlotType", "Save");
        PlayerPrefs.Save();
        SetSlotText(slots, 0);
    }

    public void LoadButton()
    {
        PlayerPrefs.SetString("SlotType", "Load");
        PlayerPrefs.Save();
        SetSlotText(slots, 0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void SetSlotText(List<Text> _slots, int _slot)
    {
        if (_slot != 0)
        {
            PlayerPrefs.SetString(_slot + "slot", System.DateTime.Now.ToShortDateString() + " " +  System.DateTime.Now.ToShortTimeString());
        }
        //Think about this, does it really need to be in a loop?
        for (int i = 1; i < _slots.Capacity; i++)
        {
            
            string textData = PlayerPrefs.GetString(i + "slot");
            

            _slots[i].text = textData;
        }
    }

    

}
