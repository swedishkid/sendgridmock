using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendgridMock.Features.Mail
{
    public class List
    {
        public class Model
        {
            public List<SendgridMock.Mail> Mails { get; set; }
            public string Date { get; set; }
        }
    }
}
