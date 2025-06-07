using Safecharge.Utils;

namespace Safecharge.Model.Common
{
    /// <summary>
    /// Holder for merchant's item info.
    /// </summary>
    public class Item
    {
        private string name;
        private string price;
        private string quantity;

        /// <summary>
        /// The name of the item.
        /// </summary>
        /// <remarks>This field is mandatory.</remarks>
        public string Name
        {
            get { return this.name; }
            set
            {
                Guard.RequiresNotNull(value, nameof(this.name));
                Guard.RequiresLengthBetween(value.Length, Constants.MinLengthStringDefault, Constants.MaxLengthStringDefault, nameof(this.Name));
                this.name = value;
            }
        }

        /// <summary>
        /// The price of a single unit of the item. Represented as a string.
        /// </summary>
        /// <remarks>This field is mandatory. Max length is 10.</remarks>
        public string Price
        {
            get { return this.price; }
            set
            {
                Guard.RequiresNotNull(value, nameof(this.price));
                Guard.RequiresMaxLength(value.Length, 10, nameof(this.Price));
                this.price = value;
            }
        }

        /// <summary>
        /// The quantity of the item. Represented as a string.
        /// </summary>
        /// <remarks>This field is mandatory. Max length is 10.</remarks>
        public string Quantity
        {
            get { return this.quantity; }
            set
            {
                Guard.RequiresNotNull(value, nameof(this.quantity));
                Guard.RequiresMaxLength(value.Length, 10, nameof(this.Quantity));
                this.quantity = value;
            }
        }
    }
}
