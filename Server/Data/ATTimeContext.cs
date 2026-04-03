using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CrownATTime.Server.Models.ATTime;

namespace CrownATTime.Server.Data
{
    public partial class ATTimeContext : DbContext
    {
        public ATTimeContext()
        {
        }

        public ATTimeContext(DbContextOptions<ATTimeContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>()
              .HasOne(i => i.TimeGuardSection)
              .WithMany(i => i.AiPromptConfigurations)
              .HasForeignKey(i => i.TimeGuardSectionsId)
              .HasPrincipalKey(i => i.TimeGuardSectionsId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .HasOne(i => i.AiPromptConfiguration)
              .WithMany(i => i.ResourceCaches)
              .HasForeignKey(i => i.DefaultAitemplate)
              .HasPrincipalKey(i => i.AiPromptConfigurationId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .HasOne(i => i.EmailTemplate)
              .WithMany(i => i.ResourceCaches)
              .HasForeignKey(i => i.DefaultEmailTemplate)
              .HasPrincipalKey(i => i.EmailTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .HasOne(i => i.NoteTemplate)
              .WithMany(i => i.ResourceCaches)
              .HasForeignKey(i => i.DefaultNoteTemplate)
              .HasPrincipalKey(i => i.NoteTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .HasOne(i => i.TeamsMessageTemplate)
              .WithMany(i => i.ResourceCaches)
              .HasForeignKey(i => i.DefaultTeamsMessageTemplate)
              .HasPrincipalKey(i => i.TeamsMessageTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .HasOne(i => i.TimeEntryTemplate)
              .WithMany(i => i.ResourceCaches)
              .HasForeignKey(i => i.DefaultTimeEntryTemplate)
              .HasPrincipalKey(i => i.TimeEntryTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate>()
              .HasOne(i => i.TeamsMessageType)
              .WithMany(i => i.TeamsMessageTemplates)
              .HasForeignKey(i => i.TeamsMessageTypeId)
              .HasPrincipalKey(i => i.TeamsMessageTypeId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>()
              .HasOne(i => i.BillingCodeCache)
              .WithMany(i => i.TimeEntryTemplates)
              .HasForeignKey(i => i.BillingCodeId)
              .HasPrincipalKey(i => i.Id)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>()
              .HasOne(i => i.EmailTemplate)
              .WithMany(i => i.TimeEntryTemplates)
              .HasForeignKey(i => i.EmailTemplateId)
              .HasPrincipalKey(i => i.EmailTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>()
              .HasOne(i => i.TeamsMessageTemplate)
              .WithMany(i => i.TimeEntryTemplates)
              .HasForeignKey(i => i.TeamsMessageTemplateId)
              .HasPrincipalKey(i => i.TeamsMessageTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .HasOne(i => i.CompanyCache)
              .WithMany(i => i.WorkflowRules)
              .HasForeignKey(i => i.CompanyId)
              .HasPrincipalKey(i => i.Id)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .HasOne(i => i.WorkflowTriggerType)
              .WithMany(i => i.WorkflowRules)
              .HasForeignKey(i => i.WorkflowTriggerTypeId)
              .HasPrincipalKey(i => i.WorkflowTriggerTypeId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.EmailTemplate)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.EmailTemplateId)
              .HasPrincipalKey(i => i.EmailTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.NoteTemplate)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.NoteTemplateId)
              .HasPrincipalKey(i => i.NoteTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.TeamsMessageTemplate)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.TeamsMessageTemplateId)
              .HasPrincipalKey(i => i.TeamsMessageTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.TimeEntryTemplate)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.TimeEntryTemplateId)
              .HasPrincipalKey(i => i.TimeEntryTemplateId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.WorkflowRule)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.WorkflowRuleId)
              .HasPrincipalKey(i => i.WorkflowRuleId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .HasOne(i => i.WorkflowStepType)
              .WithMany(i => i.WorkflowSteps)
              .HasForeignKey(i => i.WorkflowStepTypeId)
              .HasPrincipalKey(i => i.WorkflowStepTypeId)
              .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>()
              .Property(p => p.SharedWithEveryone)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.AiPromptConfiguration>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.SendAsTech)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.ShareWithOthers)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.NotifyTicketContact)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.NotifyTicketAdditionalContacts)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.NotifyTicketPrimaryResource)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.EmailTemplate>()
              .Property(p => p.NotifyTicketSecondaryResources)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.LiveLink>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.LiveLink>()
              .Property(p => p.ShareWithOthers)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.LiveLink>()
              .Property(p => p.RequiresConfirmationToRun)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.NoteTemplate>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ChecklistItemsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.EmailNotesCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.CompanyDetailsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.AichatCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ContactDetailsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.RocketshipCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.DeviceDetailsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.HideTimeDetails)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ItgluePasswordsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ItglueDocumentsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ItglueFlexibleAssetsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.ItglueConfigurationsCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.ResourceCache>()
              .Property(p => p.LiveLinksCollapsed)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate>()
              .Property(p => p.ShareWithOthers)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TeamsMessageType>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.IsNonBillable)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.ShowOnInvoice)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.IsCompleted)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.TimeStampStatus)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntryTemplate>()
              .Property(p => p.ShareWithOthers)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeGuardSection>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .Property(p => p.RuleOrder)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .Property(p => p.StepOrder)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStep>()
              .Property(p => p.ConfirmationDialogContinueOnNo)
              .HasDefaultValueSql(@"((0))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowStepType>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowTriggerType>()
              .Property(p => p.Active)
              .HasDefaultValueSql(@"((1))");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.DateWorked)
              .HasColumnType("datetimeoffset");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.StartDateTime)
              .HasColumnType("datetimeoffset");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.EndDateTime)
              .HasColumnType("datetimeoffset");

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.HoursWorked)
              .HasPrecision(18,2);

            builder.Entity<CrownATTime.Server.Models.ATTime.TimeEntry>()
              .Property(p => p.OffsetHours)
              .HasPrecision(18,2);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .Property(p => p.TimeEntryHoursWorkedGreaterThan)
              .HasPrecision(18,2);

            builder.Entity<CrownATTime.Server.Models.ATTime.WorkflowRule>()
              .Property(p => p.TimeEntryHoursWorkedLessThan)
              .HasPrecision(18,2);
            this.OnModelBuilding(builder);
        }

        public DbSet<CrownATTime.Server.Models.ATTime.AiPromptConfiguration> AiPromptConfigurations { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.AllowedTicketStatus> AllowedTicketStatuses { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.BillingCodeCache> BillingCodeCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.CompanyCache> CompanyCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.ContractCache> ContractCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.EmailTemplate> EmailTemplates { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.LiveLink> LiveLinks { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.NoteTemplate> NoteTemplates { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.ResourceCache> ResourceCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.RoleCache> RoleCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.ServiceDeskRoleCache> ServiceDeskRoleCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TeamsMessageTemplate> TeamsMessageTemplates { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TeamsMessageType> TeamsMessageTypes { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TicketEntityPicklistValueCache> TicketEntityPicklistValueCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TicketNoteEntityPicklistValueCache> TicketNoteEntityPicklistValueCaches { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TimeEntry> TimeEntries { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TimeEntryTemplate> TimeEntryTemplates { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.TimeGuardSection> TimeGuardSections { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.WorkflowRule> WorkflowRules { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.WorkflowStep> WorkflowSteps { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.WorkflowStepType> WorkflowStepTypes { get; set; }

        public DbSet<CrownATTime.Server.Models.ATTime.WorkflowTriggerType> WorkflowTriggerTypes { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.SqlServerOnDeleteConvention));
        }
    }
}