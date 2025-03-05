using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_07_PRN222_A02.BLL.DTOs
{
    public class SystemAccountDTO
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public byte AccountRole { get; set; }

    }
    public class SystemAccountDTOAdd
    {
        public string AccountName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountRole { get; set; }
        public string AccountPassword { get; set; }

    }

}
