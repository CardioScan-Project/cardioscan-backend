﻿using CardioScanAPI.Areas.User.Models.User;
using ChatBot.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using ChatBot.API.Areas.User.Data;

namespace CardioScanAPI.Areas.User.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet("doctor-info")]
        public async Task<IActionResult> DoctorInfoById(int doctorId)
        {
            var servicio = new DUser();

            var data = await servicio.GetDoctorInfo(doctorId);

            return Ok(data);
        }

        [HttpPost("patient")]
        public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
        {
            var servicio = new DUser();

            var response = await servicio.CreatePatient(patient);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.INCORRECTO)
                return BadRequest(response.Respuesta);

            return Ok(response.Respuesta);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int doctorId, string newPassword)
        {
            var servicio = new DUser();

            var response = await servicio.ChangePassword(doctorId, newPassword);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.INCORRECTO)
                return BadRequest(response.Respuesta);

            return Ok(response.Respuesta);
        }

        [AllowAnonymous]
        [HttpGet("recover-password")]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            var servicio = new DUser();

            var response = await servicio.ValidateEmail(email);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.INCORRECTO)
                return BadRequest(response.Respuesta);

            var mensaje = new MailMessage
            {
                From = new MailAddress("vasquezsolis.goycochea@gmail.com", "CardioScan"),
                Subject = "Recuperación de credenciales",
                Body = $"Hola {response.Name},<br/><br/>Recibimos una solicitud para restablecer su contraseña.<br/>" +
                           $"Usuario: {response.Email}<br/>" +
                           $"Para restablecer por favor utiliza el siguiente enlace: <br/>" +
                           $"<a href='https://www.google.com/'>CardioScan IA</a><br/><br/>" +
                           $"Saludos,<br/>El equipo de CardioScan.",
                IsBodyHtml = true
            };

            mensaje.To.Add(new MailAddress(response.Email, response.Name));

            using (var cliente = new SmtpClient("smtp.gmail.com", 587))
            {
                cliente.UseDefaultCredentials = false;
                cliente.Credentials = new NetworkCredential("vasquezsolis.goycochea@gmail.com", "uzapnftkybmjyzgo");
                cliente.EnableSsl = true;

                await cliente.SendMailAsync(mensaje);
            }

            return Ok(response.Respuesta);
        }

        [HttpGet("patient-list")]
        public async Task<IActionResult> GetPatientList(int doctorId, string firstName, string lastName, string dni)
        {
            var servicio = new DUser();

            var data = await servicio.GetPatientList(doctorId, firstName, lastName, dni);

            return Ok(data);
        }

        [HttpPost("add-prenancy")]
        public async Task<IActionResult> AddPrenancy([FromBody] Pregnancy pregnancy)
        {
            var servicio = new DUser();

            var response = await servicio.AddPrenancy(pregnancy.PatientId, pregnancy.LastMenstrualPeriod);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.INCORRECTO)
                return BadRequest(response.Respuesta);

            return Ok(response.Respuesta);
        }

        [HttpGet("pregnancies")]
        public async Task<IActionResult> GetPregnancies(int patientId)
        {
            var servicio = new DUser();

            var data = await servicio.GetPregnancies(patientId);

            return Ok(data);
        }

        [HttpGet("screening-list")]
        public async Task<IActionResult> GetScreeningList(int doctorId, int patientId, string date)
        {
            var servicio = new DUser();

            var data = await servicio.GetScreeningList(doctorId, patientId, date);

            return Ok(data);
        }

        [HttpGet("echocardiographies")]
        public async Task<IActionResult> EchocardiographiesByScreening(int screeningId)
        {
            var servicio = new DUser();

            var data = await servicio.EchocardiographiesByScreening(screeningId);

            return Ok(data);
        }

        [HttpPost("echocardiography")]
        public async Task<IActionResult> AddEchocardiography(int screeningId, [FromBody] Echocardiography echocardiography)
        {
            var servicio = new DUser();

            echocardiography.ScreeningId = screeningId;

            var response = await servicio.AddEchocardiography(echocardiography);

            if (response.Codigo == ConstantHelpers.Response.Respuesta.INCORRECTO)
                return BadRequest(response.Respuesta);

            return Ok(response.Respuesta);
        }
    }
}
