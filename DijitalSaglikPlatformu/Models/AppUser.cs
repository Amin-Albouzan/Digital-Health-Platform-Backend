﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DijitalSaglikPlatformu.Models
{
    public class AppUser : IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; }
        public DoctorProfile DoctorProfile { get; set; }
    }
}
