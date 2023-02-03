using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButton;

    public Sprite tabIdle, tabHover, tabActive;
    public Color tabIdleColor, tabHoverColor, tabActiveColor;

    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;
    public void Subscribe(TabButton button)
    {
        if (tabButton == null) tabButton = new List<TabButton>();

        tabButton.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabColor();
        //ResetTab();
        if(selectedTab == null || button != selectedTab)
        {
            // button.backGround.sprite = tabHover;
            button.backGroundColor = tabHoverColor;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabColor();
        //ResetTab();

       // button.backGround.sprite = tabIdle;
      //  button.backGroundColor = tabIdleColor;
    }
    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null) selectedTab.DeSelect();

        selectedTab = button;

        selectedTab.Select();

        ResetTabColor();
        //ResetTab();

       // button.backGround.sprite = tabActive;
        button.backGroundColor = tabActiveColor;

        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTab()
    {
        foreach (TabButton button in tabButton)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.backGround.sprite = tabIdle;
        }
    }
    public void ResetTabColor()
    {
        foreach (TabButton button in tabButton) button.backGroundColor = tabIdleColor;
    }
}
