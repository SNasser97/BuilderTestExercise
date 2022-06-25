﻿using BuilderTestSample.Exceptions;
using BuilderTestSample.Model;

namespace BuilderTestSample.Services
{
    public class OrderService
    {
        public void PlaceOrder(Order order)
        {
            ValidateOrder(order);

            ExpediteOrder(order);

            AddOrderToCustomerHistory(order);
        }

        private void ValidateOrder(Order order)
        {
            // throw InvalidOrderException unless otherwise noted.

            // TODO: order ID must be zero (it's a new order)
            if (order.Id != 0) throw new InvalidOrderException("Order ID must be 0.");

            // TODO: order amount must be greater than zero
            if (order.TotalAmount <= 0) throw new InvalidOrderException("Order amount must be greater than 0.");
            // TODO: order must have a customer (customer is not null)
            if (order.Customer is null) throw new InvalidOrderException("Order cannot have null customer.");

            ValidateCustomer(order.Customer);
        }

        private void ValidateCustomer(Customer customer)
        {
            // throw InvalidCustomerException unless otherwise noted
            // create a CustomerBuilder to implement the tests for these scenarios

            // TODO: customer must have an ID > 0
            if (customer.Id <= 0) throw new InvalidCustomerException("Customer Id must be greater than zero");
            // TODO: customer must have an address (it is not null)
            if (customer.HomeAddress is null) throw new InvalidCustomerException("Customer Address cannot be null");
            // TODO: customer must have a first and last name
            if (string.IsNullOrWhiteSpace(customer.FirstName) || string.IsNullOrWhiteSpace(customer.LastName)) throw new InvalidCustomerException("Customer must have firstname and lastname");
            // TODO: customer must have credit rating > 200 (otherwise throw InsufficientCreditException)
            if (customer.CreditRating <= 200) throw new InsufficientCreditException("Credit rating must be greater than 200");
            // TODO: customer must have total purchases >= 0
            if (customer.TotalPurchases < 0) throw new InvalidCustomerException("Total purchases must be zero or higher");

            ValidateAddress(customer.HomeAddress);
        }

        private void ValidateAddress(Address homeAddress)
        {
            // throw InvalidAddressException unless otherwise noted
            // create an AddressBuilder to implement the tests for these scenarios

            // TODO: street1 is required (not null or empty)
            if (string.IsNullOrWhiteSpace(homeAddress.Street1)) throw new InvalidAddressException("StreetOne cannot be null or empty");
            // TODO: city is required (not null or empty)
            // TODO: state is required (not null or empty)
            // TODO: postalcode is required (not null or empty)
            // TODO: country is required (not null or empty)
        }

        private void ExpediteOrder(Order order)
        {
            // TODO: if customer's total purchases > 5000 and credit rating > 500 set IsExpedited to true
        }

        private void AddOrderToCustomerHistory(Order order)
        {
            // TODO: add the order to the customer

            // TODO: update the customer's total purchases property
        }
    }
}
