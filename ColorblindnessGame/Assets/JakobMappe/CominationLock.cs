using UnityEngine;

public class CominationLock : MonoBehaviour
{
    public GameObject unlockText;

    public Sprite sprite1, sprite2, sprite3;
    public SpriteRenderer symbol1, symbol2, symbol3;

    public bool unlocked = false;
    public bool unlockSpawned =false;

    public int digit1, digit2, digit3;


    // Update is called once per frame
    void Update()
    {
        if (!unlocked)
        {
            // DIGITS GOES: 0 = LEAF, 1 = HOUSE, 2 = FLOWER
            //Combination is: [0, 2, 1]
            
            if(digit1 == 0) //First symbol is LEAF
            {
                if(digit2 == 2) //Second symbol is FLOWER
                {
                    if(digit3 == 1) //Third symbol is HOUSE
                    {
                        unlocked = true; //Unlocked!
                    }
                }
            }
        }


        if (unlocked && !unlockSpawned)
        {
            unlockText.SetActive(true);
            unlockSpawned = true;
            Debug.Log("You win!");
        }
    }

    public void ChangeDigit1()
    {
        digit1++;
        if(digit1 > 2)
        {
            digit1 = 0;
        }

        switch (digit1)
        {
            case 0:
                symbol1.sprite = sprite1; //LEAF
                break;
            case 1:
                symbol1.sprite = sprite3; //HOUSE
                break;
            case 2:
                symbol1.sprite = sprite2; //FLOWER
                break;
            default:
                symbol1.sprite = sprite1; //Default to LEAF
                break;
        }
    }

    public void ChangeDigit2()
    {
        digit2++;
        if (digit2 > 2)
        {
            digit2 = 0;
        }

        switch (digit2)
        {
            case 0:
                symbol2.sprite = sprite1; //LEAF
                break;
            case 1:
                symbol2.sprite = sprite3; //HOUSE
                break;
            case 2:
                symbol2.sprite = sprite2; //FLOWER
                break;
            default:
                symbol2.sprite = sprite1; //Default to LEAF
                break;
        }
    }

    public void ChangeDigit3()
    {
        digit3++;
        if (digit3 > 2)
        {
            digit3 = 0;
        }

        switch (digit3)
        {
            case 0:
                symbol3.sprite = sprite1; //LEAF
                break;
            case 1:
                symbol3.sprite = sprite3; //HOUSE
                break;
            case 2:
                symbol3.sprite = sprite2; //FLOWER
                break;
            default:
                symbol3.sprite = sprite1; //Default to LEAF
                break;
        }
    }
}
