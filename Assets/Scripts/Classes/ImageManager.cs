using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum Side { NOTHING = 0, LEFT = 1, MIDDLE = 2, RIGHT = 3, BG = 4}
public enum ImgType { BG, CHAR }

public class ImageManager : MonoBehaviour {

    public static Side char1Side = Side.NOTHING;
    public static Side char2Side = Side.NOTHING;
    public static Side char3Side = Side.NOTHING;

    private Object[] charSprites;
    private Object[] bgSprites;

    [SerializeField]
    private Animator charAnimLeft;
    [SerializeField]
    private Animator charAnimMiddle;
    [SerializeField]
    private Animator charAnimRight;
    [SerializeField]
    private Animator bgAnim;


    public Animator CharAnimLeft
    {
        get
        {
            return charAnimLeft;
        }

        set
        {
            charAnimLeft = value;
        }
    }

    public Animator CharAnimMiddle
    {
        get
        {
            return charAnimMiddle;
        }

        set
        {
            charAnimMiddle = value;
        }
    }

    public Animator CharAnimRight
    {
        get
        {
            return charAnimRight;
        }

        set
        {
            charAnimRight = value;
        }
    }

    public Animator BgAnim
    {
        get
        {
            return bgAnim;
        }

        set
        {
            bgAnim = value;
        }
    }

    // Use this for initialization
    void Start () {
        charSprites = Resources.LoadAll("Sprites/Characters", typeof(Sprite)).Cast<Sprite>().ToArray();
        bgSprites = Resources.LoadAll("Sprites/Backgrounds", typeof(Sprite)).Cast<Sprite>().ToArray();

    }

    public Sprite AddSpriteImage(string imgName, ImgType type)
    {

        switch (type) {
            case ImgType.CHAR:
                foreach (Sprite image in charSprites)
                {
                    // Look for a sprite with the same name, usually from a text line
                    if (imgName == image.name)
                    {
                        return image;
                    }
                }
                break;
            case ImgType.BG:
                foreach (Sprite image in bgSprites)
                {

                    if (imgName == image.name)
                    {
                        return image;
                        
                    }
                }
                break;
        }
       
        Debug.Log("image not found");
        return null;
    }


    public void DisableAnimations(int charIndex, Side _side)
    {
        switch (_side)
        {
            case Side.LEFT:
                foreach (var parameter in charAnimLeft.parameters)
                {
                    charAnimLeft.SetBool(parameter.name, false);
                    DialogueManager.leftUp = "false";
                }
                break;
            case Side.MIDDLE:
                foreach (var parameter in charAnimMiddle.parameters)
                {
                    charAnimMiddle.SetBool(parameter.name, false);
                    DialogueManager.middleUp = "false";                    
                }
                break;
            case Side.RIGHT:
                foreach (var parameter in charAnimRight.parameters)
                {
                    charAnimRight.SetBool(parameter.name, false);
                    DialogueManager.rightUp = "false";
                }
                break;
            case Side.BG:
                foreach (var parameter in bgAnim.parameters)
                {
                    bgAnim.SetBool(parameter.name, false);
                }
                break;
        }
        switch (charIndex)
        {
            case 1:
                char1Side = Side.NOTHING;
                break;
            case 2:
                char2Side = Side.NOTHING;
                break;
            case 3:
                char3Side = Side.NOTHING;
                break;
        }

    }

}
