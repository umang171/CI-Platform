﻿using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.DataModels;

public partial class ResetPassword
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
