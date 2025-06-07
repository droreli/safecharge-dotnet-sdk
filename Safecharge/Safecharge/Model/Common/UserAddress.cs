using Safecharge.Utils;

namespace Safecharge.Model.Common
{
    /// <summary>
    /// Holder for user address information.
    /// </summary>
    public class UserAddress
    {
        private string firstName;
        private string lastName;
        private string email;
        private string phone;
        private string address;
        private string city;
        private string country;
        private string state;
        private string zip;
        private string cell;
        private string county;

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string FirstName
        {
            get { return this.firstName; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthFirstName, nameof(this.FirstName));
                this.firstName = value;
            }
        }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string LastName
        {
            get { return this.lastName; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthLastName, nameof(this.LastName));
                this.lastName = value;
            }
        }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthEmail, nameof(this.Email));
                this.email = value;
            }
        }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string Phone
        {
            get { return this.phone; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthPhone, nameof(this.Phone));
                this.phone = value;
            }
        }

        /// <summary>
        /// The primary street address line.
        /// </summary>
        public string Address
        {
            get { return this.address; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthAddress, nameof(this.Address));
                this.address = value;
            }
        }

        /// <summary>
        /// The city of the address.
        /// </summary>
        public string City
        {
            get { return this.city; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthCity, nameof(this.City));
                this.city = value;
            }
        }

        /// <summary>
        /// The two-letter ISO country code (e.g., US, GB).
        /// </summary>
        public string Country
        {
            get { return this.country; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthCountry, nameof(this.Country));
                Guard.RequiresValidCountryCode(value, nameof(this.Country));
                this.country = value;
            }
        }

        /// <summary>
        /// The state or province of the address.
        /// </summary>
        public string State
        {
            get { return this.state; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthState, nameof(this.State));
                this.state = value;
            }
        }

        /// <summary>
        /// The postal or ZIP code.
        /// </summary>
        public string Zip
        {
            get { return this.zip; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthZip, nameof(this.Zip));
                this.zip = value;
            }
        }

        /// <summary>
        /// The cell phone number of the user.
        /// </summary>
        public string Cell
        {
            get { return this.cell; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthPhone, nameof(this.Cell));
                this.cell = value;
            }
        }

        /// <summary>
        /// The county of the address.
        /// </summary>
        public string County
        {
            get { return this.county; }
            set
            {
                Guard.RequiresMaxLength(value?.Length, Constants.MaxLengthCounty, nameof(this.County));
                this.county = value;
            }
        }

        /// <summary>
        /// Indicates if the AVS check resulted in a match. (e.g., Y, N, X)
        /// This property might be more relevant in a response context.
        /// </summary>
        public string AddressMatch { get; set; }

        /// <summary>
        /// The second line of the street address.
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// The third line of the street address.
        /// </summary>
        public string AddressLine3 { get; set; }

        /// <summary>
        /// The home phone number of the user.
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// The work phone number of the user.
        /// </summary>
        public string WorkPhone { get; set; }
    }
}
