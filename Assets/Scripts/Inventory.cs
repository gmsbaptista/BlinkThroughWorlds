﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public const int numItemSlots = 6;
    public Image[] itemImages = new Image[numItemSlots];
    public Item[] items = new Item[numItemSlots];
    private int numItems = 0;

    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (!itemImages[i].enabled)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                numItems++;
                return;
            }
        }
    }
    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                numItems--;
                return;
            }
        }
    }

    public bool ItemInInventory (Item itemToFind)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToFind)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFull()
    {
        return numItems >= numItemSlots;
    }
}