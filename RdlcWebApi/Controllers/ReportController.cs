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
                        }).ToListAsync();

            var user = new UserDto { };
            foreach (var result in myData)
            {
                user = new UserDto { control_number = result.ControlNumber, full_name = result.FirstName + " " + result.MiddleName + " " + result.PaternalName + " " + result.MaternalName };
            }
            return user;
        }


        private string getReportName(string type, string subType)
        {
            var outputFileName = type + subType + ".pdf";
            return outputFileName;
        }

    }
}
