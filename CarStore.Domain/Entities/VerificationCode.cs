using CarStore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Entities
{
    public class VerificationCode : BaseEntity<int>
    {
        public string Email { get; set; } = string.Empty;
        public string CodeHash { get; set; } = string.Empty;
        public VerificationCodePurpose Purpose { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresOn { get; set; }

        public bool IsUsed { get; set; }
        public bool IsVerified { get; set; }

        public int AttemptsCount { get; set; }
    }
}
