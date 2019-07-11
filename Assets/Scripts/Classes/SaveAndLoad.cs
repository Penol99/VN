using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class SaveAndLoad : MonoBehaviour {

    public static bool loading;
   

    public void SaveGame(int slot)
    {
        DialogueManager dMan = GetComponent<DialogueManager>();
        int textFile = dMan.CTextFileIndex;
        int textLine = dMan.PrevTestLineIndex;

        string dialogueName = dMan.nameText.text;
        string bgSprite = dMan.BG.sprite.name;

        string leftChar = (dMan.charImageL.sprite == null) ? "Empty" : dMan.charImageL.sprite.name;
        string middleChar = (dMan.charImageM.sprite == null) ? "Empty" : dMan.charImageM.sprite.name;
        string rightChar = (dMan.charImageR.sprite == null) ? "Empty" : dMan.charImageR.sprite.name;

        string leftUp = DialogueManager.leftUp;
        string middleUp = DialogueManager.middleUp;
        string rightUp = DialogueManager.rightUp;

        string trackIndex = AudioManager.currentTrack.ToString();
        if (DialogueManager.IsNullOrWhiteSpace(dialogueName))
        {
            dialogueName = "Empty";
        } 

        string[] saveData = { textFile.ToString(), textLine.ToString(), dialogueName, bgSprite, leftChar, middleChar, rightChar, leftUp, middleUp, rightUp, trackIndex};

        System.IO.File.WriteAllLines(GetSlot(slot), saveData);
    }

    public void LoadGame(int slot)
    {
        loading = true;
        StartCoroutine(loadingTime(.5f));
        DialogueManager dMan = GetComponent<DialogueManager>();
        string[] tempData = System.IO.File.ReadAllLines(GetSlot(slot));
        // IF more things needs to be saved, raise this from 4 to a higher value
        string[] loadData = new string[11];
        int whiteSpaceCount = 0;

        for (int i = 0; i < tempData.Length; i++)
        {

            if (!String.IsNullOrEmpty(tempData[i]))
            {  
                loadData[whiteSpaceCount] = tempData[i];
                whiteSpaceCount += 1;
            }

        }

        int trackIndex = System.Convert.ToInt32(loadData[10]);
        int storedFile = System.Convert.ToInt32(loadData[0]);
        int storedLine = System.Convert.ToInt32(loadData[1]);
        string storedName = loadData[2];
        string storedBG = loadData[3];

        dMan.audioMan.PlayTrack(trackIndex);
        dMan.LoadTextFile(storedFile);
        dMan.EndDialogue();
        dMan.StartDialogue(storedFile, storedLine);
        //for (int i = 0; i < storedLine+1; i++)
        //{
        //   dMan.DialogueChange(true);
        //}
        dMan.CTextLineIndex = storedLine;   
        dMan.DialogueChange(true);
        dMan.BG.sprite = dMan.imgMan.AddSpriteImage(storedBG, ImgType.BG);
        dMan.nameText.text = (storedName == "Empty") ? "" : storedName;

        if (loadData[4] != "Empty")
        {
            dMan.charImageL.sprite = dMan.imgMan.AddSpriteImage(loadData[4], ImgType.CHAR);
        }
        if (loadData[5] != "Empty")
        {
            dMan.charImageM.sprite = dMan.imgMan.AddSpriteImage(loadData[5], ImgType.CHAR);
        }
        if (loadData[6] != "Empty")
        {
            dMan.charImageR.sprite = dMan.imgMan.AddSpriteImage(loadData[6], ImgType.CHAR);
        }

               

        if (loadData[7] == "true") {
            dMan.imgMan.CharAnimLeft.SetBool("slideUp", true);
        }
        if (loadData[8] == "true")
        {
            dMan.imgMan.CharAnimMiddle.SetBool("slideUp", true);
        }
        if (loadData[9] == "true")
        {
            dMan.imgMan.CharAnimRight.SetBool("slideUp", true);
        }

    }

    public string GetSlot(int _slot)
    {
        System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "/Saves");
        string directory = @System.IO.Directory.GetCurrentDirectory() + "/Saves" + "/Slot" + _slot + ".txt";
    
        return directory;
    }


    public IEnumerator loadingTime(float time)
    {
        yield return new WaitForSeconds(time);
        loading = false;
    }

}
