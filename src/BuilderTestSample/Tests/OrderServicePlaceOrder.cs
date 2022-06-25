using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace BuilderTestSample.Tests
{
    public class OrderServicePlaceOrder
    {
        private readonly OrderService _orderService = new ();
        private readonly OrderBuilder _orderBuilder = new ();
        private readonly CustomerBuilder _customerBuilder = new ();
        private readonly AddressBuilder _addressBuilder = new ();

        /*
            Order exception checks - tests ValidateOrder
        */
        [Fact]
        public void ThrowsInvalidOrderExceptionGivenOrderWithExistingId()
        {
            var order = _orderBuilder
                            .WithId(123)
                            .Build();

            InvalidOrderException invalidOrderException = AssertOnException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order ID must be 0.", invalidOrderException.Message);
        }

        [Fact]
        public void ThrowsInvalidOrderExceptionGivenOrderWithAmountOfZero()
        {
            Order order = _orderBuilder
                            .WithId(0)
                            .WithAmount(0)
                            .Build();

            InvalidOrderException invalidOrderException = AssertOnException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order amount must be greater than 0.", invalidOrderException.Message);
        }

        [Fact]
        public void DoesNotThrowInvalidOrderExceptionGivenOrderWithAmountOfOneHundred()
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState("state")
                    .WithPostalCode("postalcode")
                    .WithCountry("country")
                    .Build();
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(address)
                    .WithFirstname("Bob")
                    .WithLastname("Doe")
                    .WithCreditRating(201)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            try
            {
                _orderService.PlaceOrder(order);
            }
            catch (InvalidOrderException ex)
            {
                throw new XunitException($"Should not throw InvalidOrderException: {ex.Message}");
            }
        }

        [Fact]
        public void ThrowsInvalidOrderExceptionGivenOrderWithNoCustomer()
        {
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .Build();

            InvalidOrderException invalidOrderException = AssertOnException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order cannot have null customer.", invalidOrderException.Message);
        }

        /*
            Customer exception checks - tests ValidateCustomer
        */
        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerIdIsZero()
        {
            Customer customer = _customerBuilder
                    .WithId(0)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer Id must be greater than zero", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerIdIsLessThanZero()
        {
            Customer customer = _customerBuilder
                    .WithId(-1)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer Id must be greater than zero", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerAddressIsNull()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(null)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer Address cannot be null", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerFirstnameAndLastnameAssignedAreEmptyStrings()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
                    .WithFirstname(string.Empty)
                    .WithLastname(string.Empty)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer must have firstname and lastname", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerFirstnameAndLastnameAssignedAreNull()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
                    .WithFirstname(null)
                    .WithLastname(null)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer must have firstname and lastname", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerFirstnameAndLastnameAssignedAreWhitespaces()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
                    .WithFirstname(" ")
                    .WithLastname(" ")
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer must have firstname and lastname", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInsufficientCreditExceptionWhenCustomerCreditRatingIsLessThanTwoHundred()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
                    .WithFirstname("Bob")
                    .WithLastname("Doe")
                    .WithCreditRating(199)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InsufficientCreditException insufficientCreditException = AssertOnException<InsufficientCreditException>(_orderService, order);
            Assert.Equal("Credit rating must be greater than 200", insufficientCreditException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionWhenCustomerTotalPurchasesIsLessThanZero()
        {

            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
                    .WithFirstname("Bob")
                    .WithLastname("Doe")
                    .WithCreditRating(201)
                    .WithTotalPurchases(-1)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidCustomerException invalidCustomerException = AssertOnException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Total purchases must be zero or higher", invalidCustomerException.Message);
        }

        [Fact]
        public void DoesNotThrowInvalidCustomerExceptionWhenCustomerDetailsAreValid()
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState("state")
                    .WithPostalCode("postalcode")
                    .WithCountry("country")
                    .Build();
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(address)
                    .WithFirstname("Bob")
                    .WithLastname("Doe")
                    .WithCreditRating(201)
                    .WithTotalPurchases(1)
                    .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            try
            {
                _orderService.PlaceOrder(order);
            }
            catch (InvalidCustomerException ex)
            {
                throw new XunitException($"Should not throw InvalidCustomerException: {ex.Message}");
            }
            catch (InsufficientCreditException ex)
            {
                throw new XunitException($"Should not throw InsufficientCreditException: {ex.Message}");
            }
        }
        /*
            Address exception checks - tests ValidateAddress
        */
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowsInvalidAddressExceptionWhenStreetOneIsNullOrEmpty(string streetOneValues)
        {
            Address address = _addressBuilder
                    .WithStreetOne(streetOneValues)
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidAddressException invalidAddressException = AssertOnException<InvalidAddressException>(_orderService, order);
            Assert.Equal("StreetOne cannot be null or empty", invalidAddressException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowsInvalidAddressExceptionWhenCityIsNullOrEmpty(string cityValues)
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity(cityValues)
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidAddressException invalidAddressException = AssertOnException<InvalidAddressException>(_orderService, order);
            Assert.Equal("City cannot be null or empty", invalidAddressException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowsInvalidAddressExceptionWhenStateIsNullOrEmpty(string stateValues)
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState(stateValues)
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidAddressException invalidAddressException = AssertOnException<InvalidAddressException>(_orderService, order);
            Assert.Equal("State cannot be null or empty", invalidAddressException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowsInvalidAddressExceptionWhenPostalcodeIsNullOrEmpty(string postalCodeValues)
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState("state")
                    .WithPostalCode(postalCodeValues)
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidAddressException invalidAddressException = AssertOnException<InvalidAddressException>(_orderService, order);
            Assert.Equal("Postalcode cannot be null or empty", invalidAddressException.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ThrowsInvalidAddressExceptionWhenCountryIsNullOrEmpty(string countryValues)
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState("state")
                    .WithPostalCode("postalcode")
                    .WithCountry(countryValues)
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            InvalidAddressException invalidAddressException = AssertOnException<InvalidAddressException>(_orderService, order);
            Assert.Equal("Country cannot be null or empty", invalidAddressException.Message);
        }

        [Fact]
        public void DoesNotThrowInvalidAddressExceptionWhenAddressIsValid()
        {
            Address address = _addressBuilder
                    .WithStreetOne("street1")
                    .WithCity("city")
                    .WithState("state")
                    .WithPostalCode("postalcode")
                    .WithCountry("country")
                    .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(201)
                   .WithTotalPurchases(1)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            try
            {
                _orderService.PlaceOrder(order);
            }
            catch (InvalidAddressException ex)
            {
                throw new XunitException($"Should not throw InvalidAddressException: {ex.Message}");
            }
        }

        /*
            Order service tests - test order service private methods  
        */
        [Fact]
        public void OrderServiceSetsExpeditedTrueOnOrderThatExceedsFiveThousandTotalPurchasesWithCreditLimitMoreThanFiveHundred()
        {
            Address address = _addressBuilder
                   .WithStreetOne("street1")
                   .WithCity("city")
                   .WithState("state")
                   .WithPostalCode("postalcode")
                   .WithCountry("country")
                   .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(501)
                   .WithTotalPurchases(5001)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();

            _orderService.PlaceOrder(order);
            Assert.True(order.IsExpedited);
        }

        [Fact]
        public void OrderServiceSetsExpeditedFalseOnOrderLessThanFiveThousandTotalPurchasesWithCreditLimitLessThanFiveHundred()
        {
            Address address = _addressBuilder
                   .WithStreetOne("street1")
                   .WithCity("city")
                   .WithState("state")
                   .WithPostalCode("postalcode")
                   .WithCountry("country")
                   .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(499)
                   .WithTotalPurchases(4999)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();

            _orderService.PlaceOrder(order);
            Assert.False(order.IsExpedited);
        }

        [Fact]
        public void OrderServiceAddsOrderForCustomer()
        {
            Address address = _addressBuilder
                   .WithStreetOne("street1")
                   .WithCity("city")
                   .WithState("state")
                   .WithPostalCode("postalcode")
                   .WithCountry("country")
                   .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(499)
                   .WithTotalPurchases(4999)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();

            _orderService.PlaceOrder(order);
            IEnumerable<Order> actualCustomerOrders = order.Customer.OrderHistory;
            Assert.Single(actualCustomerOrders);

            Order actualOrder = actualCustomerOrders.FirstOrDefault();
            Assert.NotNull(actualOrder);
            Assert.Equal(0, actualOrder.Id);

            Customer actualCustomer = actualOrder.Customer;
            Assert.Equal(1, actualCustomer.Id);
            Assert.Equal("Bob", actualCustomer.FirstName);
            Assert.Equal("Doe", actualCustomer.LastName);
            Assert.Equal(499, actualCustomer.CreditRating);
            Assert.Equal(4999, actualCustomer.TotalPurchases);

            Address actualAddress = actualCustomer.HomeAddress;
            Assert.Equal("street1", actualAddress.Street1);
            Assert.Null(actualAddress.Street2);
            Assert.Null(actualAddress.Street3);
            Assert.Equal("city", actualAddress.City);
            Assert.Equal("state", actualAddress.State);
            Assert.Equal("postalcode", actualAddress.PostalCode);
            Assert.Equal("country", actualAddress.Country);
        }

        [Fact]
        public void OrderServiceAddsTwoOrdersForCustomer()
        {
            Address address = _addressBuilder
                   .WithStreetOne("street1")
                   .WithCity("city")
                   .WithState("state")
                   .WithPostalCode("postalcode")
                   .WithCountry("country")
                   .Build();
            Customer customer = _customerBuilder
                   .WithId(1)
                   .WithHomeAddress(address)
                   .WithFirstname("Bob")
                   .WithLastname("Doe")
                   .WithCreditRating(499)
                   .WithTotalPurchases(4999)
                   .Build();
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .WithCustomer(customer)
                    .Build();
            Order order2 = _orderBuilder
                    .WithId(0)
                    .WithAmount(90m)
                    .WithCustomer(customer)
                    .Build();

            _orderService.PlaceOrder(order);
            _orderService.PlaceOrder(order2);
            IEnumerable<Order> actualCustomerOrders = order.Customer.OrderHistory;
            Assert.NotEmpty(actualCustomerOrders);
            Assert.Equal(2, actualCustomerOrders.Count());

            var expectedOrders = new List<Order> { order, order2 };

            foreach (Order actualOrder in actualCustomerOrders)
            {
                Order expectedOrder = expectedOrders.FirstOrDefault(eo => eo.TotalAmount == actualOrder.TotalAmount);
                Assert.NotNull(expectedOrder);
                Assert.Equal(expectedOrder.TotalAmount, actualOrder.TotalAmount);
            }
        }

        private TException AssertOnException<TException>(OrderService orderService, Order order)
where TException : Exception
        {
            
            TException exception = Assert.Throws<TException>(() => orderService.PlaceOrder(order));
            return exception;
        }
    }
}
