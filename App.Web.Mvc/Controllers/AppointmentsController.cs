﻿using App.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Mvc.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiAddress = "http://localhost:5005/api/Appointments";

        public AppointmentsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // GET: AppointmentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppointmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Appointment collection)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiAddress, collection);
                TempData["Message"] = "<div class='alert alert-success'>Your Appointment has been created... Thank you for choosing us</div>";
                return RedirectToAction("Index","Home");
            }
            catch
            {
                return View();
            }
        }

       
    }
}
