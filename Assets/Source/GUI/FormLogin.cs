// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormLogin : FormWidget
{
    [Header("Form Fields")]
    [SerializeField]
    private InputField inputFieldEmail = null;
    [SerializeField]
    private InputField inputFieldPass = null;


    protected override void Awake()
    {
        base.Awake();
    }


    public override void Validate(Action<Dictionary<string, string>> onSuccess, Action<string> onFail)
    {
        Debug.AssertFormat(inputFieldEmail != null && inputFieldPass != null, "Failed to validate FormLogin. One of more inputs are null.");

        // Validate the form before logging in
        bool bEmailEmpty = Validators.IsStringEmpty(inputFieldEmail.text);
        bool bEmailInvalid = Validators.IsInvalidEmail(inputFieldEmail.text);
        bool bPassEmpty = Validators.IsStringEmpty(inputFieldPass.text);
        if (bEmailEmpty || bEmailInvalid || bPassEmpty)
        {
            string errorMsg = "";
            if (bEmailEmpty)
                errorMsg += "Email field is empty.\n";
            if (bEmailInvalid)
                errorMsg += "Email is invalid.\n";
            if (bPassEmpty)
                errorMsg += "Password field is empty.\n";

            if (onFail != null)
                onFail.Invoke(errorMsg);

            return;
        }

        if (onSuccess != null)
        {
            Dictionary<string, string> pak = new Dictionary<string, string>();
            pak.Add("email", inputFieldEmail.text);
            pak.Add("password", inputFieldPass.text);
            onSuccess.Invoke(pak);
        }
    }
}
