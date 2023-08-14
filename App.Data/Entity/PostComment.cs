﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Data.Entity.BaseEntities;

namespace App.Data.Entity
{
    public class PostComment : BaseAuditEntity
    {




        [ForeignKey(nameof(PostId))]
        public int? PostId { get; set; }
        public Post? Post { get; set; }


        [ForeignKey("User")]
        public int? UserId { get; set; }
        public User? User { get; set; }

        [DataType(DataType.Text), Column(TypeName = "nvarchar(100)"), MaxLength(200, ErrorMessage = "The {0} cannot exceed 200 characters."), MinLength(5, ErrorMessage = "The {0} must be at least 5 characters.")]
        public string? FullName { get; set; } = "Guest";

        [DataType(DataType.EmailAddress), EmailAddress, Column(TypeName = "varchar(200)"), MaxLength(200, ErrorMessage = "The {0} cannot exceed 200 characters."), MinLength(5, ErrorMessage = "The {0} must be at least 5 characters.")]
        public string? Email { get; set; }

        [Column(TypeName = "text"), DataType(DataType.Text)]
        public string Comment { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
