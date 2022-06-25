using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;
using BuilderTestSample.Services;
using BuilderTestSample.Tests.TestBuilders;
using System;
using Xunit;
using Xunit.Sdk;

namespace BuilderTestSample.Tests
{
    public class OrderServicePlaceOrder
    {
        private readonly OrderService _orderService = new ();
        private readonly OrderBuilder _orderBuilder = new ();
        private readonly CustomerBuilder _customerBuilder = new ();

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
            Customer customer = _customerBuilder
                    .WithId(1)
                    .WithHomeAddress(new Address())
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
            catch (InvalidOrderException invalidOrderException)
            {
                throw new XunitException($"Should not throw InvalidOrderException: {invalidOrderException.Message}");
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

        private TException AssertOnException<TException>(OrderService orderService, Order order)
where TException : Exception
        {
            
            TException exception = Assert.Throws<TException>(() => orderService.PlaceOrder(order));
            return exception;
        }
    }
}
