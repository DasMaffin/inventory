using System;
using UnityEngine;

/// <summary>
/// Inventory factory and builder. Use Inventory.Create() to create a new inventory.
/// </summary>
public class Inventory
{
    private GameObject
        owner;    // The owner of the inventory.
    private uint
        capacity = 0;    // How many slots in the inventory.
    private bool
        canOpen = true;
    private InventorySlot[]
        slots;    // The slots in the inventory.

    private Inventory() { }

    /// <summary>
    /// Creates a new inventory instance.
    /// </summary>
    /// <returns>Returns the new inventory instance.</returns>
    public static Inventory Create()
    {
        return new Inventory();
    }

    /// <summary>
    /// Sets the owner of the inventory. This is usually the player or entity that holds the inventory.
    /// </summary>
    /// <param name="owner">The object owning the inventory.</param>
    public Inventory SetOwner(GameObject owner)
    {
        if(owner == null)
        {
            Debug.LogWarning("Inventory owner is null. This is probably unintended and should be fixed.\nIf this is intended remove this method from the inventory.");
            return this;
        }
        this.owner = owner;
        return this;
    }

    /// <summary>
    /// The amount of slots in the inventory.
    /// </summary>
    /// <param name="capacity">The amount of slots in the inventory.</param>
    public Inventory SetCapacity(uint capacity)
    {
        this.capacity = capacity;
        this.slots = new InventorySlot[capacity];
        return this;
    }

    /// <summary>
    /// Whether or not the inventory can be opened. Default is true.
    /// If true the inventory can be opened by clicking on its owner.
    /// </summary>
    /// <param name="canOpen"></param>
    /// <returns></returns>
    public Inventory SetCanOpen(bool canOpen)
    {
        this.canOpen = canOpen;
        return this;
    }
}