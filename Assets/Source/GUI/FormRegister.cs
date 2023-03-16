// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormRegister : FormWidget
{
    [Header("Form Fields")]
    [SerializeField]
    private InputField inputFieldName = null;
    [SerializeField]
    private InputField inputFieldEmail = null;
    [SerializeField]
    private InputField inputFieldPass = null;
    [SerializeField]
    private InputField inputFieldConfirmPass = null;


    protected override void Awake()
    {
        base.Awake();
    }


    public override void Validate(Action<Dictionary<string, string>> onSuccess, Action<string> onFail)
    {
        // Validate the form before creating user
        bool bNameEmpty = Validators.IsStringEmpty(inputFieldName.text);
        bool bEmailEmpty = Validators.IsStringEmpty(inputFieldEmail.text);
        bool bEmailInvalid = Validators.IsInvalidEmail(inputFieldEmail.text);
        bool bPassBadLength = Validators.NotBelowMinLength(inputFieldPass.text, 5);
        bool bPassNoMatch = Validators.ArePasswordsNotEqual(inputFieldPass.text, inputFieldConfirmPass.text);
        if (bNameEmpty || bEmailEmpty || bEmailInvalid || bPassBadLength || bPassNoMatch)
        {
            string errorMsg = "";
            if (bNameEmpty)
                errorMsg += "Name field is empty.\n";
            if (bEmailEmpty)
                errorMsg += "Email field is empty.\n";
            if (bEmailInvalid)
                errorMsg += "Email is invalid.\n";
            if (bPassBadLength)
                errorMsg += "Password needs to have at least 5 characters.\n";
            if (bPassNoMatch)
                errorMsg += "Passwords do not match.";

            if (onFail != null)
                onFail.Invoke(errorMsg);
            return;
        }

        if (onSuccess != null)
        {
            Dictionary<string, string> pak = new Dictionary<string, string>();
            pak.Add("name", inputFieldName.text);
            pak.Add("email", inputFieldEmail.text);
            pak.Add("password", inputFieldPass.text);
            onSuccess.Invoke(pak);
        }
    }
}
