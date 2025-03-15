    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
            [Required(ErrorMessage = "Name is required.")]
            public string AccountName { get; set; }
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email format.")]
            public string AccountEmail { get; set; }
            [Required(ErrorMessage = "Password is required.")]
            public byte AccountRole { get; set; }
            [Required(ErrorMessage = "Password is required.")]
            public string? AccountPassword { get; set; }

        }

    }
