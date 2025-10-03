using System.Collections;
using System.Linq;
using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    // Arrange
    string testItemsPath = "Examples/"; // Relative to parenting "Resources" folder. Open ended where all the assets are.

    [Test]
    public void InventoryBuilder_InitializesCorrectly()
    {
        uint slotCount = 10;
        Inventory inventory = MockInventory.CreateInventory(slotCount);

        // Act
        var inventorySlots = typeof(Inventory)
            .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(inventory) as InventorySlot[];

        // Assert
        Assert.IsNotNull(inventorySlots, "Slots array should not be null, but is null.");
        Assert.AreEqual(slotCount, inventorySlots.Length, $"Inventory should contain the correct number of slots.");

        // Optional: Check each slot is initialized
        foreach (var slot in inventorySlots)
        {
            Assert.IsNotNull(slot, "Each slot should be initialized");
        }
    }

    [Test]
    public void Inventory_GetAllItems()
    {
        // Arrange
        Inventory.SetItemsPath(testItemsPath);
        // Act
        var items = Inventory.AllItems; // Don't use reflection here, use the getter.
        // Assert
        Assert.IsNotNull(items, "AllItems should not be null.");
        Assert.IsNotEmpty(items, "AllItems should contain items when the path is correct and items exist.");
        Assert.IsTrue(items.Count == 3, $"AllItems should load 3 test items. Loaded {items.Count}.");
    }

    [Test]
    public void Inventory_AddItem_ReturnsRemainingAmountWhenFull()
    {
        // Arrange
        Inventory.SetItemsPath(testItemsPath);
        uint InventorySize = 5;
        Inventory inventory = MockInventory.CreateInventory(InventorySize);

        uint firstAmountToAdd = 8;
        uint remainder = inventory.AddItem(Inventory.AllItems[0], firstAmountToAdd); // Add more than one stack size. Less than two full stacks.
        Assert.AreEqual(0, remainder, "Should return 0 as the inventory has space and should fill all available.");
        var inventorySlots = typeof(Inventory)
            .GetField("slots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .GetValue(inventory) as InventorySlot[];
        for (int i = 0; i < inventorySlots.Count(); i++)
        {
            if (i < 2)
                Assert.NotNull(inventorySlots[i], $"Slot {i} should be full with any items.");
            else
                Assert.NotNull(inventorySlots[i], $"Slot {i} should be empty.");
        }

        uint secondAmountToAdd = 50; // Fill the inventory completely.
        remainder = inventory.AddItem(Inventory.AllItems[0], secondAmountToAdd); // Add more than one stack size. Less than two full stacks.
        uint expectedRemainder = secondAmountToAdd - (Inventory.AllItems[0].StackSize * InventorySize) + firstAmountToAdd;
        Assert.AreEqual(expectedRemainder, remainder, "Should return 42 as the inventory should be full and unable to add more.");
    }
}

public class MockInventory
{
    public static Inventory CreateInventory(uint capacity)
    {
        return InventoryBuilder.Create().SetCapacity(capacity).Build();
    }
}
