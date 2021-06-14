using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for declaring the quality of the item
/// </summary>
public enum Quality {Common, Uncommon, Rare, Epic }

/// <summary>
/// Superclass for all items
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    /// <summary>
    /// Icon used when moving and placing the items
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// The size of the stack, less than 2 is not stackable
    /// </summary>
    [SerializeField]
    private int stackSize;

    /// <summary>
    /// The item's title
    /// </summary>
    [SerializeField]
    private string titel;

    /// <summary>
    /// The item's quality
    /// </summary>
    [SerializeField]
    private Quality quality;

    /// <summary>
    /// A reference to the slot that this item is sitting on
    /// </summary>
    private SlotScript slot;

    public string MyTitel
    {
        get
        {
            return titel;
        }
    }

    /// <summary>
    /// Property for accessing the icon
    /// </summary>
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    /// <summary>
    /// Property for accessing the stacksize
    /// </summary>
    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    /// <summary>
    /// Proprty for accessing the slotscript
    /// </summary>
    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    /// <summary>
    /// Returns a description of this specific item
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        string color = string.Empty;

        switch (quality)
        {
            case Quality.Common:
                color = "#d6d6d6";
                break;
            case Quality.Uncommon:
                color = "#00ff00ff";
                break;
            case Quality.Rare:
                color = "#0000ffff";
                break;
            case Quality.Epic:
                color = "#800080ff";
                break;
        }

        return string.Format("<color={0}>{1}</color>", color, titel);
    }

    /// <summary>
    /// Removes the item from the inventory
    /// </summary>
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}

