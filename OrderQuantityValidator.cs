namespace VP_Midterm_Project
{
    internal static class OrderQuantityValidator
    {
        internal static bool TryGetPositiveQuantity(string value, out int quantity)
        {
            return int.TryParse(value, out quantity) && quantity > 0;
        }
    }
}
