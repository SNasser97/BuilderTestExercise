using BuilderTestSample.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderTestSample.Tests
{
    public class CustomerBuilder
    {
        private Customer _customer;

        public CustomerBuilder WithId(int id)
        {
            this.InitialiseCustomerWithId(id);
            return this;
        }

        public CustomerBuilder WithFirstname(string firstname)
        {
            this._customer.FirstName = firstname;
            return this;
        }

        public CustomerBuilder WithLastname(string lastname)
        {
            this._customer.LastName = lastname;
            return this;
        }

        public CustomerBuilder WithHomeAddress(Address homeAddress)
        {
            this._customer.HomeAddress = homeAddress;
            return this;
        }

        public CustomerBuilder WithCreditRating(int creditRating)
        {
            this._customer.CreditRating = creditRating;
            return this;
        }

        public CustomerBuilder WithTotalPurchases(decimal totalPurchases)
        {
            this._customer.TotalPurchases = totalPurchases;
            return this;
        }

        public CustomerBuilder WithOrderHistory(IEnumerable<Order> previousOrders)
        {
            this._customer.OrderHistory = (List<Order>)previousOrders;
            return this;
        }

        public Customer Build()
        {
            return this._customer;
        }

        private void InitialiseCustomerWithId(int id)
        {
            this._customer = new Customer(id);
        }
    }
}
