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

        // added comment
        [Fact]
        public void ThrowsInvalidOrderExceptionGivenOrderWithExistingId()
        {
            var order = _orderBuilder
                            .WithId(123)
                            .Build();

            InvalidOrderException invalidOrderException = AssertOrderException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order ID must be 0.", invalidOrderException.Message);
        }

        [Fact]
        public void ThrowsInvalidOrderExceptionGivenOrderWithAmountOfZero()
        {
            Order order = _orderBuilder
                            .WithId(0)
                            .WithAmount(0)
                            .Build();

            InvalidOrderException invalidOrderException = AssertOrderException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order amount must be greater than 0.", invalidOrderException.Message);
        }

        [Fact]
        public void DoesNotThrowInvalidOrderExceptionGivenOrderWithAmountOfOneHundred()
        {
            Customer customer = _customerBuilder
                    .WithId(1)
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

            InvalidOrderException invalidOrderException = AssertOrderException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order cannot have null customer.", invalidOrderException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionGivenOrderWithCustomerIdOfZero()
        {
            Customer customer = _customerBuilder.WithId(0)
                .Build();
            Order order = _orderBuilder
                .WithId(0)
                .WithAmount(100m)
                .WithCustomer(customer)
                .Build();

            InvalidCustomerException invalidCustomerException = AssertOrderException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer Id cannot be zero.", invalidCustomerException.Message);
        }

        [Fact]
        public void ThrowsInvalidCustomerExceptionGivenOrderWithCustomerIdOfLessThanZero()
        {
            Customer customer = _customerBuilder.WithId(-1)
                .Build();
            Order order = _orderBuilder
                .WithId(0)
                .WithAmount(100m)
                .WithCustomer(customer)
                .Build();

            InvalidCustomerException invalidCustomerException = AssertOrderException<InvalidCustomerException>(_orderService, order);
            Assert.Equal("Customer Id cannot be zero.", invalidCustomerException.Message);
        }


        private TException AssertOrderException<TException>(OrderService orderService, Order order)
            where TException : Exception
        {

            TException exception = Assert.Throws<TException>(() => orderService.PlaceOrder(order));
            return exception;
        }
    }
}
