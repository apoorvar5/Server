using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[PrimaryKey("ClubId", "PlayerId")]
[Table("PlayerClub")]
public partial class PlayerClub
{
    [Key]
    public int ClubId { get; set; }

    [Key]
    public int PlayerId { get; set; }

    [ForeignKey("ClubId")]
    [InverseProperty("PlayerClub")]
    public virtual Club Club { get; set; } = null!;

    [ForeignKey("PlayerId")]
    [InverseProperty("PlayerClub")]
    public virtual Player Player { get; set; } = null!;
}