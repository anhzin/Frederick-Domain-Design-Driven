﻿// ***********************************************************************
// Assembly         : FrederickNguyen.Api
// Author           : thangnd
// Created          : 06-21-2018
//
// Last Modified By : thangnd
// Last Modified On : 06-21-2018
// ***********************************************************************
// <copyright file="CustomerController.cs" company="FrederickNguyen.Api">
//     Copyright (c) by adguard. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Linq;
using FrederickNguyen.ApplicationLayer.Models;
using FrederickNguyen.ApplicationLayer.Services;
using FrederickNguyen.DomainCore.Commands;
using FrederickNguyen.DomainCore.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FrederickNguyen.WebApi.Controllers
{
    /// <summary>
    /// Class CustomerController.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Produces("application/json")]
    [Route("api/customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController" /> class.
        /// </summary>
        /// <param name="customerService">The customer service.</param>
        /// <param name="notificationHandler">The notification handler.</param>
        /// <param name="commandBusHandler">The command bus handler.</param>
        public CustomerController(ICustomerService customerService, INotificationHandler<DomainNotification> notificationHandler,
            ICommandDispatcher commandBusHandler) : base(notificationHandler, commandBusHandler)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// check email is available.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("email")]
        public IActionResult IsEmailAvailable(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return BadRequest(new { IsSuccessStatusCode = false, Errors = "Email is required" });

                var result = _customerService.IsEmailAvailable(email);
                return Ok(new { IsSuccessStatusCode = true, Results = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }

        /// <summary>
        /// Gets the customer by identifier.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("id")]
        public IActionResult GetCustomerById(Guid customerId)
        {
            try
            {
                if (customerId == Guid.Empty) return BadRequest(new { IsSuccessStatusCode = false, Errors = "Customer id is required" });

                var result = _customerService.GetCustomerById(customerId);
                return Ok(new { IsSuccessStatusCode = true, Results = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }

        /// <summary>
        /// Gets the purchases history.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet("purchases")]
        public IActionResult GetPurchasesHistory(Guid customerId)
        {
            try
            {
                if (customerId == Guid.Empty) return BadRequest(new { IsSuccessStatusCode = false, Errors = "Customer id is required" });

                var result = _customerService.GetCustomerPurchaseHistory(customerId);
                return Ok(new { IsSuccessStatusCode = true, Results = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost("Add")]
        public IActionResult AddCustomer([FromBody] AddNewCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { IsSuccessStatusCode = false, Errors = ModelState });

                _customerService.Add(model);

                if (Errors.Any()) return BadRequest(new { IsSuccessStatusCode = false, Errors });
                return Ok(new { IsSuccessStatusCode = true });
            }
            catch (Exception ex)
            {
                return NotFound(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }

        /// <summary>
        /// Updates the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPatch("update")]
        public IActionResult UpdateCustomer([FromBody] UpdateCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { IsSuccessStatusCode = false, Errors = ModelState });

                _customerService.Update(model);

                if (Errors.Any()) return BadRequest(new { IsSuccessStatusCode = false, Errors });
                return Ok(new { IsSuccessStatusCode = true });
            }
            catch (Exception ex)
            {
                return NotFound(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }

        /// <summary>
        /// Deletes the customer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>IActionResult.</returns>
        [HttpDelete("delete")]
        public IActionResult DeleteCustomer([FromBody] RemoveCustomerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new { IsSuccessStatusCode = false, Errors = ModelState });

                _customerService.Remove(model);

                if (Errors.Any()) return BadRequest(new { IsSuccessStatusCode = false, Errors });
                return Ok(new { IsSuccessStatusCode = true });
            }
            catch (Exception ex)
            {
                return NotFound(new { IsSuccessStatusCode = false, Errors = ex.Message });
            }
        }
    }
}