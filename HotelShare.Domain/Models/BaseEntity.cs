using System;
using System.ComponentModel.DataAnnotations;

namespace HotelShare.Domain.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}