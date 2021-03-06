using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace GsEPWv8_5_MVC.Core.Entity
{
    public class ForgotPassword
    {
        public string user_id { get; set; }
        public string email_id { get; set; }
        public string new_pwd { get; set; }
        public string Show_day { get; set; }
        public string CompanyAppName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyWebLink { get; set; }
        public IList<ForgotPassword> ListCheckUserIDExist { get; set; }
    }
}
