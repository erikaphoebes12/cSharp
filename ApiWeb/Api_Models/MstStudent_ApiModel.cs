using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWeb.Api_Models
{
    public class MstStudent_ApiModel
    {
        public int Id { get; set; }
        public string StudentCode { get; set; }
        public string FullName { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }
    }
}