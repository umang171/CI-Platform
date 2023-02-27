﻿using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.DataModels;

public partial class MissionRating
{
    public long MissionRatingId { get; set; }

    public long UserId { get; set; }

    public long MissionId { get; set; }

    public byte Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual User2 User { get; set; } = null!;
}
