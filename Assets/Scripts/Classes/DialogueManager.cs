using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {
    //------------------------------------//
    //------------------------------------//
    // *** NOTES *** // ------------------//
    /*
     * if a datatype has 'c' before its name it means current, example: cText = currentText.
     * strings with the value of ":^)" are just set to placeholder values to avoid errors
     * 
     * FIX MONOLOGUE EFFECT THINGAMAJIG
     * 
     * 
     */
    //------------------------------------//
    //------------------------------------//
    //------------------------------------//
    // ** Variables ** //
    //Public
    public ImageManager imgMan;
    public SaveAndLoad saveLoadMan;
    public AudioManager audioMan;
    public TextAsset[] textFiles;
    public Text uiText;
    public Text nameText;
    public Text choice1Text;
    public Text choice2Text;
    public Text choice3Text;
    public Text choice4Text;
    public Image charImageL;
    public Image charImageM;
    public Image charImageR;
    public Image BG;
    public GameObject DiagTick;
    public GameObject dialogueCanvas;
    public GameObject button3;
    public GameObject button4;
    public GameObject exitPanel;
    public GameObject choicePanel;
    public GameObject textPanel;
    public int endTextLine;   
    public float typeSpeed;
    public static string leftUp = "false";   
    public static string middleUp = "false";
    public static string rightUp = "false";


    // Static

    //Private
    private TextAsset cTextFile;
    private string[] textLines;
    private string[] cChoices = {"","","","",""};
    private string[] storedChoices = new string[100];
    private string cName;
    private bool isTypingText;
    private bool cancelTypingText;
    private bool dialogueOpen;
    private bool choiceBoxOpen;
    private bool mouseButton;
    private int cChoiceIndex = 1;
    private int cTextFileIndex;
    private int cTextLineIndex;
    private int prevTestLineIndex;

    public int CTextFileIndex
    {
        get
        {
            return cTextFileIndex;
        }

        set
        {
            cTextFileIndex = value;
        }
    }

    public int CTextLineIndex
    {
        get
        {
            return cTextLineIndex;
        }

        set
        {
            cTextLineIndex = value;
        }
    }

    public int PrevTestLineIndex
    {
        get
        {
            return prevTestLineIndex;
        }

        set
        {
            prevTestLineIndex = value;
        }
    }


    // Static

    // ** Unity Methods ** //

    private void Start()
    {
        saveLoadMan = GetComponent<SaveAndLoad>();
        audioMan = GetComponent<AudioManager>();
        for (int i = 0; i < storedChoices.Length; i++)
        {
            if (storedChoices[i] == null)
            {               
                storedChoices[i] = ":^)";
            }
        }
        if (PlayerPrefs.GetInt("NewGame") == 1)
        {
            StartDialogue(0, 0);
            DialogueChange(false);
        } else
        {
            saveLoadMan.LoadGame(PlayerPrefs.GetInt("SlotLoaded"));
        }
        
        PlayerPrefs.SetInt("NewGame", 0);
        PlayerPrefs.Save();
        
    }

    private void Update()
    {



        Vector3 mPos = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        
        // Disable mouse button when choice box is open so that you cant just go to the next dialogue
        if (!choiceBoxOpen && (mPos.x < 4f || mPos.y > -4.4f) && dialogueCanvas.activeSelf && !exitPanel.activeSelf)
        {
            mouseButton = Input.GetMouseButtonDown(0);
        }
        // What line on the text file will the dialogue start
        // These keys are just for the development process and debugging DONT FORGET TO REMOVE THESE WHEN DONE

        if (Input.GetKeyDown(KeyCode.S))
        {

            EndDialogue();
        }


        if (dialogueOpen)
        {
            LoadTextToUi();
            DiagTick.SetActive(!isTypingText);
        }

        choicePanel.SetActive(choiceBoxOpen);

        if (mouseButton && dialogueOpen)
        {
            DialogueChange(false);
        }
        

    }
    // ** Methods ** //

    // Private
    
    // This needs to be optimised because it literally writes the same shit on every images so it doesnt matter
    // One way to fix this is using a universal charather image object and having it assign its values to the
    // left rigt and middle ones based on the enum _side
    // Animations-------------------------------------------------------------
    private void SpriteAnimations(Side _side)
    {
        if (textLines[cTextLineIndex + 2].Contains("[SLIDE UP]"))
        {
            switch (_side)
            {
                case Side.LEFT:
                    imgMan.CharAnimLeft.SetBool("slideUp", true);
                    leftUp = "true";
                    break;
                case Side.MIDDLE:
                    imgMan.CharAnimMiddle.SetBool("slideUp", true);
                    middleUp = "true";
                    break;
                case Side.RIGHT:
                    imgMan.CharAnimRight.SetBool("slideUp", true);
                    rightUp = "true";
                    break;
            }
        }

        if (textLines[cTextLineIndex + 2].Contains("[TALK]"))
        {
            switch (_side)
            {
                case Side.LEFT:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimLeft, "talk", .2f));
                    break;
                case Side.MIDDLE:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimMiddle, "talk", .2f));
                    break;
                case Side.RIGHT:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimRight, "talk", .2f));
                    break;
            }
        }
        if (textLines[cTextLineIndex + 2].Contains("[BANG SHAKE]"))
        {
            switch (_side)
            {
                case Side.LEFT:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimLeft, "bangShakeL", .5f));
                    break;
                case Side.MIDDLE:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimMiddle, "bangShakeM", .5f));
                    break;
                case Side.RIGHT:
                    StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.CharAnimRight, "bangShakeR", .5f));
                    break;
            }
        }
        if (_side == Side.BG)
        {
            if (textLines[cTextLineIndex + 1].Contains("[SCREEN SHAKE]"))
            {

                StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.BgAnim, "screenShake" , .5f));
                cTextLineIndex += 1;
            }
            if (textLines[cTextLineIndex + 1].Contains("[FADE IN]"))
            {
                StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.BgAnim, "fadeIn" , .6f));
                cTextLineIndex += 1;
            }
            if (textLines[cTextLineIndex + 1].Contains("[FADE OUT]"))
            {
                StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.BgAnim, "fadeOut", .6f));
                cTextLineIndex += 1;
            }
            if (textLines[cTextLineIndex + 1].Contains("[FADE IN WHITE]"))
            {
                StartCoroutine(StartAndDisableAnimationOnEnd(imgMan.BgAnim, "fadeInWhite", .6f));
                cTextLineIndex += 1;
            }
        }
        
    }

    public IEnumerator StartAndDisableAnimationOnEnd(Animator _anim, string boolName, float _time)
    {
        // Only works if clip has set exit time on end transition
        _anim.SetBool(boolName, true);
        yield return new WaitForSeconds(_time);
        _anim.SetBool(boolName, false);
    }


    public void DialogueChange(bool loaded)
    {
        LoadTextToUi();
                

        endTextLine = textLines[cTextLineIndex].Length;
        if (loaded)
        {
            cancelTypingText = true;
            isTypingText = false;
            uiText.text = textLines[CTextLineIndex];
            cTextLineIndex += 1;
            
        }
        if (!loaded && !isTypingText && (textLines[cTextLineIndex].Length <= endTextLine))
        {
            StartCoroutine(TextScroll(textLines[cTextLineIndex]));
            prevTestLineIndex = CTextLineIndex;
            cTextLineIndex += 1;
            // Add the name to the UI
            nameText.text = cName;

        } else if (isTypingText && !cancelTypingText && !loaded)
        {
            
            cancelTypingText = true;
            uiText.text = textLines[cTextLineIndex];
        }
       

        if (textLines[cTextLineIndex-1].Contains("-+-") && CTextLineIndex != 0)
        {
            EndDialogue();
            Debug.Log("End");
        }
    }

    // Public

    public void LoadTextFile(int index)
    {
        cTextFile = textFiles[index];

        // Split text from text file into the array.
        textLines = cTextFile.text.Split('\n');
        // Remove blank lines from the array and comments (//)
        int tempIndex = 0;
        for (int i = 0; i < cTextFile.text.Split('\n').Length; i++)
        {
            if (!IsNullOrWhiteSpace(cTextFile.text.Split('\n')[i]) && !cTextFile.text.Split('\n')[i].Contains("//"))
            {
                textLines[tempIndex] = cTextFile.text.Split('\n')[i];
                tempIndex += 1;
            }
        }
        for (int i = tempIndex; i < textLines.Length; i++)
        {
            textLines[i] = ":^)";
        }
    }


    public void LoadTextToUi()
    {
        //--The dialogue needs to trigger the same time a new dialogue triggers, or else the old one will be the first one.
        // Skip blank lines
        //if (IsNullOrWhiteSpace(textLines[cTextLineIndex]))
        //{
        //    cTextLineIndex += 1;
        //}
        // Blank out the name assignment symbols
        if (textLines[cTextLineIndex].Contains(";;-"))
        {
            cName = textLines[cTextLineIndex];
            cName = cName.Replace(";;-", "");

            cTextLineIndex += 1;
        }
        // MONOLOGUE STUFF HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        if (textLines[CTextLineIndex][0] == '{')
        {
            textLines[cTextLineIndex] = textLines[cTextLineIndex].Replace("{","");
        }
        // This is also a giant mess, need to fix this somehow, maybe the same technique i described in the 
        // animations section would work here too, and combine them.
        if (textLines[cTextLineIndex].Contains("char_") || textLines[cTextLineIndex].Contains("[REMOVE]"))
        {
            Side tempSide = Side.NOTHING;
            int cChar = 0;
            if (textLines[cTextLineIndex + 1].Contains("[LEFT]"))
            {
                // If theres a sprite to be loaded
                if (textLines[cTextLineIndex].Contains("char"))
                {
                    charImageL.sprite = imgMan.AddSpriteImage(textLines[cTextLineIndex].Trim(), ImgType.CHAR);
                }
                tempSide = Side.LEFT;
            }
            else if (textLines[cTextLineIndex + 1].Contains("[MIDDLE]"))
            {
                if (textLines[cTextLineIndex].Contains("char"))
                {
                    charImageM.sprite = imgMan.AddSpriteImage(textLines[cTextLineIndex].Trim(), ImgType.CHAR);
                }
                tempSide = Side.MIDDLE;

            }
            else if (textLines[cTextLineIndex + 1].Contains("[RIGHT]"))
            {
                if (textLines[cTextLineIndex].Contains("char"))
                {
                    charImageR.sprite = imgMan.AddSpriteImage(textLines[cTextLineIndex].Trim(), ImgType.CHAR);
                }
                tempSide = Side.RIGHT;
            }

            if (ImageManager.char1Side == Side.NOTHING)
            {
                ImageManager.char1Side = tempSide;
                cChar = 1;
            }
            else if (ImageManager.char1Side != Side.NOTHING && ImageManager.char2Side == Side.NOTHING)
            {
                ImageManager.char2Side = tempSide;
                cChar = 2;
            }
            else if (ImageManager.char3Side == Side.NOTHING)
            {
                ImageManager.char3Side = tempSide;
                cChar = 3;
            }
            SpriteAnimations(tempSide);
            if (textLines[cTextLineIndex].Contains("char"))
            {
                cTextLineIndex += 3;
            }
            if (textLines[cTextLineIndex].Contains("[REMOVE]"))
            {
                Debug.Log("AA");
                imgMan.DisableAnimations(cChar, tempSide);
                cTextLineIndex += 2;
            }
        }
        // Backgrounds
        if (textLines[cTextLineIndex].Contains("BG_"))
        {
            BG.sprite = imgMan.AddSpriteImage(textLines[cTextLineIndex].Trim(), ImgType.BG);
            SpriteAnimations(Side.BG);
            
            cTextLineIndex += 1;
        }
        if (textLines[cTextLineIndex].Contains("SONG #"))
        {
            string songIndex = textLines[cTextLineIndex].Replace("SONG #","");
            audioMan.PlayTrack((System.Convert.ToInt32(songIndex)));
            cTextLineIndex += 1;
        }

        if (!SaveAndLoad.loading)
        {
            
            // Handle Choices
            if (textLines[cTextLineIndex].Trim().Contains("%%"))
            {
                // Normalize the string and the convert it to an int which will display the number after %% in the text files
                int choiceAmount = System.Convert.ToInt32(textLines[cTextLineIndex].Trim()[2].ToString().Normalize());
                for (int i = 0; i < choiceAmount; i++)
                {
                    Debug.Log(i);
                    cChoices[i] = textLines[cTextLineIndex + i + 1];

                }
                cTextLineIndex += choiceAmount + 2;
                StartChoices(cChoices, choiceAmount);
            }

            if (textLines[cTextLineIndex].Contains("(End Choice)"))
            {
                // If there is another choice beneath
                if (textLines[cTextLineIndex + 1].Contains("("))
                {
                    for (int i = 0; i < storedChoices.Length; i++)
                    {
                        if (!textLines[cTextLineIndex + 1].Contains(storedChoices[i]))
                        {
                            for (int j = cTextLineIndex + 2; j < textLines.Length; j++)
                            {
                                if (textLines[j].Contains("(End Choice)"))
                                {
                                    cTextLineIndex = j;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    cTextLineIndex += 1;
                }

            }
        } else
        {
            imgMan.DisableAnimations(0, Side.BG);
        }
    }

    public static bool IsNullOrWhiteSpace(string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return true;
        }

        return String.IsNullOrEmpty(value.Trim());
    }

    public void StartDialogue(int fileIndex,int startLine)
    {
        if (!dialogueOpen)
        {
            textPanel.SetActive(true);
            cTextLineIndex = startLine;
            dialogueOpen = true;
            cancelTypingText = true;
            LoadTextFile(fileIndex);
        }
    }
    public void EndDialogue()
    {
        dialogueOpen = false;
        cancelTypingText = false;
        isTypingText = false;
        textPanel.SetActive(false);
        cTextFileIndex = 0;
        cTextLineIndex = 0;
        cChoiceIndex = 1;
        for (int i = 1; i < 4; i++)
        {
            foreach (Side sides in Enum.GetValues(typeof(Side)))
            {
                imgMan.DisableAnimations(i, sides);
            }
            
        }       
    }

    public void StartChoices(string[] choices,int choiceAmount)
    {
        // Only need button3 and 4 because they are the only ones that get disabled.       
        choiceBoxOpen = true;
        switch (choiceAmount)
        {
            case 2:
                choice1Text.text = cChoices[0];
                choice2Text.text = cChoices[1];
                button3.SetActive(false);
                button4.SetActive(false);
                break;
            case 3:
                choice1Text.text = cChoices[0];
                choice2Text.text = cChoices[1];
                choice3Text.text = cChoices[2];
                button3.SetActive(true);
                button4.SetActive(false);
                break;
            case 4:
                choice1Text.text = cChoices[0];
                choice2Text.text = cChoices[1];
                choice3Text.text = cChoices[2];
                choice4Text.text = cChoices[3];
                button3.SetActive(true);
                button4.SetActive(true);
                break;
        }
        
    }
    public void ChoiceSelected(int choiceNumber)
    {
        
        // This will look like (1)#1 if the player selected the first choice on the first choice pick in the game.
        storedChoices[cChoiceIndex] = "("+cChoiceIndex.ToString()+")" + "#" + choiceNumber.ToString();
        
        for (int i = cTextLineIndex; i < textLines.Length; i++)
        {
            if (textLines[i].Contains(storedChoices[cChoiceIndex]))
            {
                
                cTextLineIndex = i+1;
                break;
            }
        }
        cChoiceIndex += 1;
        choiceBoxOpen = false;
        DialogueChange(false);
    }

    // ** Coroutines ** //

    private IEnumerator TextScroll(string lineOfText)
    {
        int cLetter = 0;
        uiText.text = "";
        isTypingText = true;
        cancelTypingText = false;
        
        while (isTypingText && !cancelTypingText && (cLetter < lineOfText.Length - 1))
        {
            uiText.text += lineOfText[cLetter];
            cLetter += 1;
            yield return new WaitForSeconds(typeSpeed/10);
        }
        uiText.text = lineOfText;
        isTypingText = false;
        cancelTypingText = false;


    }


}
