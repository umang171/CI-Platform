using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.DataModels;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? NotificationMessage { get; set; }

    public int? MessageId { get; set; }

    public bool? Status { get; set; }

    public int? UserId { get; set; }

    public string? NotificationType { get; set; }

    public int? FromUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? NotificationImage { get; set; }
}
