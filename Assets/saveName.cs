using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using PF.Actions;

public class saveName : MonoBehaviour
{
    private int value =0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void submitName()
    {
        string name = GameObject.Find("InputField").GetComponent<InputField>().text;
        Debug.Log("Saving " + name);
        value = System.Convert.ToInt32(name);
        Debug.Log("Value: " + value);

    }

    public int getValue()
    {
        return value;
    }


}
