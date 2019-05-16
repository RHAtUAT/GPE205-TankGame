using UnityEngine;

public class RememberSelection : MonoBehaviour
{
    private GameObject selection;

    void Update()
    {
        //if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject != selection)
        //    selection = EventSystem.current.currentSelectedGameObject;
        //else if (selection != null && EventSystem.current.currentSelectedGameObject == null)
        //    EventSystem.current.SetSelectedGameObject(selection);
    }
}
