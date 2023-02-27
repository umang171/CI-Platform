﻿using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.DataModels;

public partial class Country
{
    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Iso { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual ICollection<Mission> Missions { get; } = new List<Mission>();

    public virtual ICollection<User2> User2s { get; } = new List<User2>();
}
