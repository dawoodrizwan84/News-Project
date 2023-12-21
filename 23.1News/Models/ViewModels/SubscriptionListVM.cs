using _23._1News.Models.Db;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class SubscriptionListVM
{
    public int SubscriptionId { get; set; }
    public decimal SubscriptionPrice { get; set; }
    public DateTime SubscriptionCreated { get; set; }
    public int SubscriptionTypeId { get; set; }
    public string SubscriptionTypeName { get; set; }
    public bool IsActive { get; set; }
    public bool PaymentComplete { get; set; }
    public string? UserId { get; set; }
}
