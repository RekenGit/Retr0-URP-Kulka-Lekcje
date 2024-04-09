using UnityEngine;

public class DoorOpenClose : MonoBehaviour, IInteractibleObiects
{
    [SerializeField] private int doorsNeededToOpen = 1;

    public void ObiectInteract()
    {
        doorsNeededToOpen--;
        if (doorsNeededToOpen == 0) GetComponent<Animator>().SetBool("DoorOpen", true);
    }

    public void ObiectToggle()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("DoorOpen", !animator.GetBool("DoorOpen"));
    }
}
