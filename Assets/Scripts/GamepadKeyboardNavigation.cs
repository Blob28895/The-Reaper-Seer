using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamepadKeyboardNavigation : MonoBehaviour
{
    public GameObject firstSelected;
    private bool usingControllerOrKeyboard = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(usingControllerOrKeyboard);
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || Input.GetButtonDown("Submit")) && !usingControllerOrKeyboard)
        {
            usingControllerOrKeyboard = true;
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
        else if ((Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) && usingControllerOrKeyboard)
        {
            usingControllerOrKeyboard = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ResetSelected()
    {
        usingControllerOrKeyboard = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
