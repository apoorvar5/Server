using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[Table("Club")]
public partial class Club
{
    [Key]
    public int ClubId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ClubName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClubLeague { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ClubCountry { get; set; } = null!;

    [InverseProperty("Club")]
    public virtual ICollection<PlayerClub> PlayerClub { get; set; } = new List<PlayerClub>();
}
