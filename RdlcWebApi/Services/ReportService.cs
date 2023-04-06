using AspNetCore.Reporting;
using RdlcWebApi.Models;
using RdlcWebApi.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RdlcWebApi.Services
{
    public interface IReportService
    {
        //byte[] GenerateReportAsync(string type, string subType, string data);
        byte[] GenerateReportAsync(string type, string subType, UserDto data);
    }

    public class ReportService : IReportService
    {
        public byte[] GenerateReportAsync(string type, string subType, UserDto data)
        {
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("RdlcWebApi.dll", string.Empty);
            //string rdlcFilePath = string.Format("{0}ReportFiles\\{1}_{2}.rdlc", fileDirPath, type, subType);
            string rdlcFilePath = string.Format("ReportFiles\\{0}_{1}.rdlc", type, subType);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");

            LocalReport report = new LocalReport(rdlcFilePath);

            // prepare data for report

            List<UserDto> userList = new List<UserDto>();

            var user = new UserDto { };
            //user = await Get(data);
            userList.Add(data);

            report.AddDataSource("dsUsers", userList);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var result = report.Execute(GetRenderType("pdf"), 1, parameters);

            return result.MainStream;
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            return renderType;
        }

    }
}
