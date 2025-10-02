namespace Maffin.InvetorySystem.enums
{
    /// <summary>
    /// The type of inventory bar.
    /// </summary>
    public enum InventoryBarType
    {
        barIsInventory,                     // The bar itself are slots in the inventory. e.g. Minecraft hotbar.
        barReferencesInventory,             // The bar references items in the inventory.
        barReferencesInventoryKeepItems     // The bar references items in the inventory, but keeps items in the bar when removed from the inventory. e.g. ARK hotbar.
    }
}