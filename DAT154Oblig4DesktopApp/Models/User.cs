using System;
using System.Collections.Generic;

namespace DAT154Oblig4DesktopApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? Staff { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
