namespace Task2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InsuranceClaim
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte RecKey { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte PolID { get; set; }

        [StringLength(10)]
        public string PolNumber { get; set; }

        [StringLength(15)]
        public string PolType { get; set; }

        [Column("Effective Date", TypeName = "date")]
        public DateTime? Effective_Date { get; set; }

        public short? DocID { get; set; }

        [StringLength(10)]
        public string DocName { get; set; }

        public byte? Submitted { get; set; }

        public byte? Outstanding { get; set; }
    }
}
