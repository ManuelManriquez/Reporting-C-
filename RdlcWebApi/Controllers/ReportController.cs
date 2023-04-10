﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RdlcWebApi.Models;
using RdlcWebApi.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using RdlcWebApi.Services;

namespace RdlcWebApi.Controllers
{
    [Route("procedure/print")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IReportService _reportService;
        private readonly EssaContext dbContext;

        public ReportController(IReportService reportService, EssaContext dbContext)
        {
            _reportService = reportService;
            this.dbContext = dbContext;
        }

        [HttpGet("{type}/{subType}/{data}")]
        public async Task<ActionResult> Get(string type, string subType, string data)
        {
            var reportNameWithLang = type + "_" + subType;

            var myData = await Get(data);

            var reportFileByteString = _reportService.GenerateReportAsync(type, subType, myData);
            return File(reportFileByteString, MediaTypeNames.Application.Octet, getReportName(reportNameWithLang, ".pdf"));
        }

        public async Task<UserDto> Get(string data)
        {
            var myData = await dbContext.Students
                        .Join(dbContext.UserAccounts,
                        s => s.UserAccountId,
                        ua => ua.Id,
                        (s, ua) => new { s, ua })
                        .Join(dbContext.Careers,
                        sua => sua.s.CareerId,
                        c => c.Id,
                        (sua, c) => new { sua, c })
                        .Join(dbContext.ServicioSocialProcedures,
                        suac => suac.sua.s.ControlNumber,
                        ssp => ssp.StudentControlNumber,
                        (suac, ssp) => new { suac, ssp })
                        .Join(dbContext.ServicioSocialLetters,
                        suacssp => suacssp.ssp.Id,
                        ssl => ssl.ServicioSocialProcedureId,
                        (suacssp, ssl) => new { suacssp, ssl }).Where(suacssp => suacssp.suacssp.suac.sua.s.ControlNumber == data)
                        .Select(result => new
                        {
                            result.suacssp.suac.sua.s.ControlNumber,
                            result.suacssp.suac.sua.ua.FirstName,
                            result.suacssp.suac.sua.ua.MiddleName,
                            result.suacssp.suac.sua.ua.PaternalName,
                            result.suacssp.suac.sua.ua.MaternalName,
                            result.ssl.StartDate,
                            result.ssl.EndDate,
                            result.ssl.AddresseeName,
                            result.ssl.AddresseePosition,
                            result.suacssp.suac.c.CareerCurriculum,
                            result.ssl.TotalHours,
                            result.ssl.HeadOfDepartmentName,
                            result.ssl.HeadOfDepartmentPosition,
                            result.ssl.StudentServiceModality,
                            result.ssl.StudentActivity,
                            result.ssl.SupervisorName,
                            result.ssl.SupervisorPosition,
                            result.ssl.ProjectName
                        }).ToListAsync();

            var user = new UserDto { };
            foreach (var result in myData)
            {
                int total_hours = 0;

                if (result.TotalHours != null)
                {
                    total_hours = (int)result.TotalHours;
                }

                DateTime currentDate = DateTime.Now;

                int current_day = currentDate.Day;
                var current_month = getMonthStr(currentDate.Month);
                int current_year = currentDate.Year;

                int startDay = result.StartDate.Value.Day;
                var startMonth = getMonthStr(result.StartDate.Value.Month);

                int startYear = result.StartDate.Value.Year;
                int endDay = result.EndDate.Value.Day;
                var endMonth = getMonthStr(result.EndDate.Value.Month);
                int endYear = result.EndDate.Value.Year;

                user = new UserDto
                {
                    control_number = result.ControlNumber,
                    full_name = result.FirstName + " " + result.MiddleName + " " + result.PaternalName + " " + result.MaternalName,
                    addressee_name = result.AddresseeName,
                    addressee_position = result.AddresseePosition,
                    career_curriculum = result.CareerCurriculum,
                    total_hours = total_hours,
                    start_date_day = startDay,
                    start_date_month = startMonth,
                    start_date_year = startYear,
                    end_date_day = endDay,
                    end_date_month = endMonth,
                    end_date_year = endYear,
                    head_of_department_name = result.HeadOfDepartmentName,
                    head_of_department_position = result.HeadOfDepartmentPosition,
                    modality = result.StudentServiceModality,
                    student_activity = result.StudentActivity,
                    supervisor_name = result.SupervisorName,
                    supervisor_position = result.SupervisorPosition,
                    project_name = result.ProjectName,
                    current_day = getNumberStr(current_day),
                    current_month = current_month,
                    current_year = current_year
                };
            }
            return user;
        }


        private string getMonthStr(int monthInt)
        {
            switch (monthInt)
            {
                case 1:
                    return "enero";
                case 2:
                    return "febrero";
                case 3:
                    return "marzo";
                case 4:
                    return "abril";
                case 5:
                    return "mayo";
                case 6:
                    return "junio";
                case 7:
                    return "julio";
                case 8:
                    return "agosto";
                case 9:
                    return "septiembre";
                case 10:
                    return "octubre";
                case 11:
                    return "noviembre";
                case 12:
                    return "diciembre";
            }
            return "";
        }

        private string getNumberStr(int number)
        {
            switch (number)
            {
                case 1:
                    return "un";
                case 2:
                    return "dos";
                case 3:
                    return "tres";
                case 4:
                    return "cuatro";
                case 5:
                    return "cinco";
                case 6:
                    return "seis";
                case 7:
                    return "siete";
                case 8:
                    return "ocho";
                case 9:
                    return "nueve";
                case 10:
                    return "diez";
                case 11:
                    return "once";
                case 12:
                    return "doce";
                case 13:
                    return "trece";
                case 14:
                    return "catorce";
                case 15:
                    return "quince";
                case 16:
                    return "dieciséis";
                case 17:
                    return "diecisiete";
                case 18:
                    return "dieciocho";
                case 19:
                    return "diecinueve";
                case 20:
                    return "veinte";
                case 21:
                    return "veintiuno";
                case 22:
                    return "veintidós";
                case 23:
                    return "veintitrés";
                case 24:
                    return "veinticuatro";
                case 25:
                    return "veinticinco";
                case 26:
                    return "veintiséis";
                case 27:
                    return "veintisiete";
                case 28:
                    return "veintiocho";
                case 29:
                    return "veintinueve";
                case 30:
                    return "treinta";
                case 31:
                    return "treinta y uno";
            }
            return "";
        }

        private string getReportName(string type, string subType)
        {
            var outputFileName = type + subType + ".pdf";
            return outputFileName;
        }

    }
}
