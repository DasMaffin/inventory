using System.Collections;
using Maffin.InvetorySystem.Builders;
using Maffin.InvetorySystem.Inventories;
using Maffin.InvetorySystem.Slots;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    [Test]
    public void Inventory_InitializesWithCorrectNumberOfSlots()
    {
        uint slotCount = 10;
        Inventory inventory = InventoryBuilder.Create().SetCapacity(slotCount).Build();

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
}
