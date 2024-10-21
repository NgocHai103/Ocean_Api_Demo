using Sea.Domain.Base;
using System;

namespace Sea.Domain.PGCampusMails;

public class ICSMail : IEntity<long>
{
    public long Id { get; set; }
    public long FromId { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; } = null!;
    public DateTime PostedDate { get; set; }
    public string? Attachment { get; set; }
    public bool? SavedByUser { get; set; }
    public bool? DeletedBySender { get; set; }
    public bool? DeletedByRecipients { get; set; }
    public bool? DeletedBySuperAdmin { get; set; }
    public string? GrpMailToCls { get; set; }
    public string? Stuids { get; set; }
    public string? Stuclassids { get; set; }
    public bool? SendToAll { get; set; }
    public string? OrgAttachment { get; set; }
    public string? SenderName { get; set; }
    public long? BatchId { get; set; }
    public DateTime? ScheduleDate { get; set; }
    public int? MailType { get; set; }
    public bool? IsPosted { get; set; }
    public bool? InProcess { get; set; }
    public bool? IsDeleted { get; set; }
    public long? PostedCount { get; set; }
    public long? BranchId { get; set; }
    public bool? Isannouncement { get; set; }
    public bool? IsForNewJoinee { get; set; }
    public string? Clsecid { get; set; }
    public bool? IsFeedback { get; set; }
    public string? FeedbackCategory { get; set; }
    public string? Documents { get; set; }
    public long CampusId { get; set; }
    public long BrandId { get; set; }
    public bool IsStarred { get; set; }
    public bool? IsMoveToInbox { get; set; }
    public bool? IsArchived { get; set; }
    public bool? IsDeleteForever { get; set; }
    public DateTime? DeleteMailExpiryDate { get; set; }
    public bool? IsSnooze { get; set; }
    public DateTime? DateofSnooze { get; set; }
    public bool? IsSchedule { get; set; }
    public bool? IsRead { get; set; }
    public bool? IsMuted { get; set; }
    public int? FlagId { get; set; }
}