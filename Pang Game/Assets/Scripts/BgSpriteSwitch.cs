using UnityEngine;

public class BgSpriteSwitch : MonoBehaviour
{
    SpriteRenderer bg;
    public Sprite[] bgs;
    private int currentActiveSprite = 0;

    private void Awake()
    {
        if (bg == null)
            bg = GetComponent<SpriteRenderer>();
    }

    public int GetSpriteIndex()
    {
        return currentActiveSprite;
    }

    public void SwitchToSprite(int newBg)
    {
        if (newBg > bgs.Length)
            newBg = bgs.Length;

        bg.sprite = bgs[newBg];
        currentActiveSprite = newBg;
    }
    [ContextMenu("NextBg")]
    public void SwitchToNextSprite()
    {
        currentActiveSprite++;
        if (currentActiveSprite > bgs.Length - 1)
            currentActiveSprite = 0;

        SwitchToSprite(currentActiveSprite);
    }
}
