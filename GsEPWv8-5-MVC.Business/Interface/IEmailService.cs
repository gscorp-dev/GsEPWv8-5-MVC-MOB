﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GsEPWv8_5_MVC.Core.Entity;

namespace GsEPWv8_5_MVC.Business.Interface
{
   public interface IEmailService
    {
        Email GetSendMailDetails(Email objSendMail);
        Email GetMailDetails(Email objSendMail);
    }
}
