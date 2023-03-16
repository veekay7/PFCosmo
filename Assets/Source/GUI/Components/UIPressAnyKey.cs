using UnityEngine;
using UnityEngine.Events;

public class UIPressAnyKey : UIWidget
{
    public UnityEvent onAnyKeyPress = new UnityEvent();


    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (onAnyKeyPress != null)
                onAnyKeyPress.Invoke();
        }
    }
}
