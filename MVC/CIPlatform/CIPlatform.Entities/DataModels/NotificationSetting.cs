using System;
using System.Collections.Generic;

namespace CIPlatform.Entities.DataModels;

public partial class NotificationSetting
{
    public long UserId { get; set; }

    public bool? RecommendedMission { get; set; }

    public bool? StoryApproved { get; set; }

    public bool? MissionAdded { get; set; }

    public bool? RecommendedStory { get; set; }

    public bool? MissionApplicationApproved { get; set; }

    public bool? ReceiveEmailNotification { get; set; }

    public virtual User User { get; set; } = null!;
}
