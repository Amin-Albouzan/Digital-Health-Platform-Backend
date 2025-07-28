using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DijitalSaglikPlatformu.Models
{
    [Owned]
    public class RefreshToken
    {

       
            [Key]
            public int Id { get; set; }

            public string Token { get; set; }
            public DateTime ExpiresOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? RevokedOn { get; set; }

            public bool IsExpired => DateTime.UtcNow > ExpiresOn;
            public bool IsActive => RevokedOn == null && !IsExpired;

            [ForeignKey("AppUser")]    
            public string UserId { get; set; }


            public AppUser User { get; set; }
        

    }
}
