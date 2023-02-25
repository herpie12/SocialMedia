﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sm.Query.Domain.Entities
{
    [Table("Post")]
    public class PostEntity
    {
        [Key]
        public Guid PostId { get; set; }
        public string Author { get; set; }
        public DateTime DatePosted { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<CommentEntity> Comments { get; set; }

    }
}