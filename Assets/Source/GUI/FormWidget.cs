using System;
using System.Collections.Generic;

public abstract class FormWidget : UIWidget
{
    public abstract void Validate(Action<Dictionary<string, string>> onSuccess, Action<string> onFail);
}
