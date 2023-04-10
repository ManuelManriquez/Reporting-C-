using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdlcWebApi.Models
{
    public class UserDto
    {
        //public Task<IEnumerable> control_number { get; set; }
        public string control_number { get; set; }
        public string full_name { get; set; }
        public string adressee_name { get; set; }
        public string adressee_position { get; set; }
        public string career_curriculum { get; set; }
        public int total_hours { get; set; }
        public int start_date_day { get; set; }
        public string start_date_month { get; set; }
        public int start_date_year { get; set; }
        public int end_date_day { get; set; }
        public string end_date_month { get; set; }
        public int end_date_year { get; set; }
        public string head_of_department_name { get; set; }
        public string head_of_department_position { get; set; }
        public string modality { get; set; }
        public string student_activity { get; set; }
        public string supervisor_position { get; set; }
        public string supervisor_name { get; set; }
        //public string Email { get; set; }
        //public string Phone { get; set; }
    }
}
