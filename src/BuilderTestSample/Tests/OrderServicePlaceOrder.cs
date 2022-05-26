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
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
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
        public void ThrowsInvalidCustomerExceptionGivenOrderWithNoCustomer()
        {
            Order order = _orderBuilder
                    .WithId(0)
                    .WithAmount(100m)
                    .Build();

            InvalidOrderException invalidOrderException = AssertOrderException<InvalidOrderException>(_orderService, order);
            Assert.Equal("Order cannot have null customer.", invalidOrderException.Message);
        }


        private TException AssertOrderException<TException>(OrderService orderService, Order order)
            where TException : Exception
        {

            TException exception = Assert.Throws<TException>(() => orderService.PlaceOrder(order));
            return exception;
        }
    }
}
