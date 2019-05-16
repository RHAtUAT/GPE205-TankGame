using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Normally cant escape from input fields if you're using a controller, this fixes that
public class InputFieldNavigator : MonoBehaviour
{
    private Selectable previousSelectable;
    private Selectable nextSelectable;

    void Start()
    {
        previousSelectable = this.GetComponent<Selectable>().FindSelectableOnUp();
        nextSelectable = this.GetComponent<Selectable>().FindSelectableOnDown();
    }

    //Make selecting the input field not skip to the other UI elements immediately
    //Couldn't find a better way of doing this
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        if (Input.GetAxis("GamePad0 Left Joystick Y") == 1)
            nextSelectable.Select();

        if (Input.GetAxis("GamePad0 Left Joystick Y") == -1)
            previousSelectable.Select();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
            StartCoroutine(Wait(.1f));
    }
}