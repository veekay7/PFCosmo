// Copyright 2019 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewProfileWindow : FormWidget
{
    [SerializeField]
    private InputField inputFieldName = null;
    [SerializeField]
    private InputField inputFieldEmail = null;


    public override void Validate(Action<Dictionary<string, string>> onSuccess, Action<string> onFail)
    {
        // Validate the form before creating user
        bool bNameEmpty = Validators.IsStringEmpty(inputFieldName.text);
        bool bEmailEmpty = Validators.IsStringEmpty(inputFieldEmail.text);
        bool bEmailInvalid = Validators.IsInvalidEmail(inputFieldEmail.text);

        if (bNameEmpty || bEmailEmpty)
        {
            string errorMsg = "";
            if (bNameEmpty)
                errorMsg += "Name field is empty.\n";
            if (bEmailEmpty)
                errorMsg += "Email field is empty.\n";
            if (bEmailInvalid)
                errorMsg += "Email is invalid.\n";

            if (onFail != null)
                onFail.Invoke(errorMsg);
            return;
        }

        if (onSuccess != null)
        {
            Dictionary<string, string> package = new Dictionary<string, string>();
            package.Add("name", inputFieldName.text);
            package.Add("email", inputFieldEmail.text);
            onSuccess.Invoke(package);
        }
    }
}
