using System.Collections.Generic;
using UnityEngine;

public class SpriteFontNumberDb : ScriptableObject
{
    [SerializeField]
    private List<Sprite> m_sprites = new List<Sprite>();


    public Sprite GetSprite(int number)
    {
        Debug.Assert(number > -1 && number < m_sprites.Count - 1);
        return m_sprites[number];
    }


    public Sprite[] GetSprites()
    {
        return m_sprites.ToArray();
    }
}
