// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Author: VinTK
using System;
using UnityEngine;

[Serializable]
public class Kana
{
    public int id;
    public Sprite sprite;
    public bool isUsed;


    public Kana(int id, Sprite sprite)
    {
        this.id = id;
        this.sprite = sprite;
        isUsed = false;
    }
}
