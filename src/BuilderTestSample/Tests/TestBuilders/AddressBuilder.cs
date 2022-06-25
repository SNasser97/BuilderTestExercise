using BuilderTestSample.Model;

namespace BuilderTestSample.Tests.TestBuilders
{
    public class AddressBuilder
    {
        private Address _address = new ();

        public AddressBuilder WithStreetOne(string streetOne)
        {
            this._address.Street1 = streetOne;
            return this;
        }

        public AddressBuilder WithStreet2(string streetTwo)
        {

            this._address.Street2 = streetTwo;
            return this;
        }

        public AddressBuilder WithStreet3(string streetThree)
        {
            this._address.Street3 = streetThree;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {

            this._address.City = city;
            return this;
        }

        public AddressBuilder WithState(string state)
        {
            this._address.State = state;
            return this;
        }

        public AddressBuilder WithPostalCode(string postalCode)
        {
            this._address.PostalCode = postalCode;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            this._address.Country = country;
            return this;
        }

        public Address Build()
        {
            return this._address;
        }
    }
}
