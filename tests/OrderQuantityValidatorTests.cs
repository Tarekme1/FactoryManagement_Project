using System;

namespace VP_Midterm_Project
{
    internal static class OrderQuantityValidatorTests
    {
        private static int Main()
        {
            AssertInvalid(null);
            AssertInvalid(string.Empty);
            AssertInvalid("0");
            AssertInvalid("-1");
            AssertInvalid("1.5");
            AssertInvalid("not a number");
            AssertValid("1", 1);
            AssertValid("25", 25);

            Console.WriteLine("OrderQuantityValidator regression checks passed.");
            return 0;
        }

        private static void AssertInvalid(string value)
        {
            int quantity;
            if (OrderQuantityValidator.TryGetPositiveQuantity(value, out quantity))
            {
                throw new InvalidOperationException("Expected invalid quantity: " + value);
            }
        }

        private static void AssertValid(string value, int expected)
        {
            int quantity;
            if (!OrderQuantityValidator.TryGetPositiveQuantity(value, out quantity) || quantity != expected)
            {
                throw new InvalidOperationException("Expected valid quantity: " + value);
            }
        }
    }
}
