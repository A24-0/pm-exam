using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace pm_exam
{
    public class OutputData
    {
        public string ServiceName { get; set; }
        public string Password { get; set; }
        public SecureString SecretPassword { get; set; }
        public int Priority { get; set; }

    }
}
