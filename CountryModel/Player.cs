using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CountryModel;

[Table("Player")]
public partial class Player
{
    [Key]
    public int PlayerId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PlayerName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PlayerPos { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string PlayerNationality { get; set; } = null!;

    [InverseProperty("Player")]
    public virtual ICollection<PlayerClub> PlayerClub { get; set; } = new List<PlayerClub>();
}
