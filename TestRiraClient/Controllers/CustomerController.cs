using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestRira.Protos;
using TestRiraClient.Models;
using static TestRira.Protos.CustomerService;

namespace TestRiraClient.Controllers
{
    public class CustomerController : Controller
    {
        HttpClient client = new HttpClient();
        public async Task<IActionResult> Index()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);

            var res = await customerService.GetAllAsync(new Empty());
            var customers = new List<CustomerModel>();
            foreach (var item in res.Items)
            {
                var customer = new CustomerModel
                {
                    Name = item.Name,
                    Family = item.Family,
                    NationalCode = item.NationalCode,
                    Id = item.Id,
                };

                customer.Name = item.Name;
                customers.Add(customer);

            }

            return View(customers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerModel customer)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);
            var customerReq = new TestRira.Protos.Customer
            {
                Name = customer.Name,
                Family = customer.Family,
                NationalCode = customer.NationalCode
            };
            var res = await customerService.PostAsync(customerReq);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);

            var idfilter = new IdFilter()
            {
                RowId = id
            };

            var customer = customerService.GetById(idfilter);

            var customerModel = new CustomerModel()
            {
                Family = customer.Family,
                NationalCode = customer.NationalCode,
                Id = id,
                Name = customer.Name
            };

            return View(customerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerModel customer)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);
            var customerReq = new TestRira.Protos.Customer
            {
                Name = customer.Name,
                Family = customer.Family,
                NationalCode = customer.NationalCode,
                Id= customer.Id
            };
            var res = await customerService.PutAsync(customerReq);

            return RedirectToAction("Index");

        }

        public IActionResult Delete(int id)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);

            var idfilter = new IdFilter()
            {
                RowId = id
            };

            var customer = customerService.GetById(idfilter);

            var customerModel = new CustomerModel()
            {
                Family = customer.Family,
                NationalCode = customer.NationalCode,
                Id = id,
                Name= customer.Name
            };

            return View(customerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CustomerModel customer)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7212", new Grpc.Net.Client.GrpcChannelOptions { HttpClient = client });

            CustomerServiceClient customerService = new CustomerServiceClient(channel);
            var customerReq = new TestRira.Protos.Customer
            {
                Name = customer.Name,
                Family = customer.Family,
                NationalCode = customer.NationalCode,
                Id=customer.Id
                
            };
            var res = await customerService.DeleteAsync(customerReq);

            return RedirectToAction("Index");

        }
    }
}
