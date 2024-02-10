namespace pm_exam
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EncryptedData")]
    public partial class EncryptedData
    {
        [Key]
        public int PasswordID { get; set; }

        public int PasswordUserID { get; set; }

        [Required]
        [StringLength(255)]
        public string ServiceName { get; set; }

        [Required]
        public byte[] EncryptedText { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] PasswordKey { get; set; }

        [Required]
        [MaxLength(16)]
        public byte[] IV { get; set; }

        public int Priority { get; set; }

        public virtual Users Users { get; set; }
    }
}
