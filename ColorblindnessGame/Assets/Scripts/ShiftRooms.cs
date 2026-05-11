using UnityEngine;

public class ShiftRooms : MonoBehaviour, Resettable
{
    public int inInnerRoom = 0;
    public int inOuterRoom = 2;

    public GameObject innerWall;
    public GameObject outerWall;


    void Start()
    {
        innerWall.SetActive(true);
        outerWall.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        inInnerRoom++;
        inOuterRoom--;

        if (inInnerRoom > 0)
        {
            innerWall.SetActive(false);
        }
        else innerWall.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        inInnerRoom--;
        inOuterRoom++;
        
        if (inOuterRoom > 0)
        {
            outerWall.SetActive(false);
        }
        else outerWall.SetActive(true);
    }

    public void Death()
    {
        inInnerRoom = 0;
        inOuterRoom = 2;
        innerWall.SetActive(true);
        outerWall.SetActive(false);
    }
}
