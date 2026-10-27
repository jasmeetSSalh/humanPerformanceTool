using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace myFlightPerformance.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SegDocument> SegDocuments { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<EventTrigger> EventTriggers { get; set; }

        public DbSet<SegDocumentJunction> SegDocumentJunctions {get; set;}
        public DbSet<ObservedEvent> ObservedEvents { get; set; }
        public DbSet<ObservedGrade> ObservedGrades { get; set; }
        public DbSet<ObservedGradesInstructorCritique> ObservedGradesInstructorCritiques { get; set; }
        public DbSet<Instructor> Instructors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Instructor>()
                .HasIndex(i => i.Username)
                .IsUnique();

            //Composite Key: a SegId can have multiple TrigIds but can't have duplicate TrigIds
            modelBuilder.Entity<SegDocumentJunction>()
            .HasKey(j => new { j.SegId, j.TrigId });


            // SegDocumentJunction: Many-to-One with SegDocument
            modelBuilder.Entity<SegDocumentJunction>()
                .HasOne<SegDocument>()
                .WithMany()
                .HasForeignKey(j => j.SegId)
                .OnDelete(DeleteBehavior.Cascade);

            // SegDocumentJunction: Many-to-One with EventTrigger
            modelBuilder.Entity<SegDocumentJunction>()
                .HasOne<EventTrigger>()
                .WithMany()
                .HasForeignKey(j => j.TrigId)
                .OnDelete(DeleteBehavior.Cascade);

            // ObservedGrade: Many-to-One with ObservedEvent
            modelBuilder.Entity<ObservedGrade>()
                .HasOne<ObservedEvent>()
                .WithMany()
                .HasForeignKey(g => g.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // ObservedGrade: Many-to-One with Observation
            modelBuilder.Entity<ObservedGrade>()
                .HasOne<Observation>()
                .WithMany()
                .HasForeignKey(g => g.ObsId)
                .OnDelete(DeleteBehavior.Cascade);

            // ObservedEvent: Many-to-One with SegDocument
            modelBuilder.Entity<ObservedEvent>()
                .HasOne<SegDocument>()
                .WithMany()
                .HasForeignKey(e => e.SegId)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure JSON conversion for ObNumbers
            modelBuilder.Entity<SegDocumentJunction>()
                .Property(s => s.ObsId)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()), // Serialize list to JSON string
                    v => JsonSerializer.Deserialize<List<int>>(v, new JsonSerializerOptions()) // Deserialize JSON string to list
                );

            // _ = modelBuilder.Entity<SegDocumentJunction>()
            //     .Property(e => e.ObsId)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, null),
            //         v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null)
            //     );

            modelBuilder.Entity<ObservedGradesInstructorCritique>()
                .HasOne<SegDocument>()
                .WithMany()
                .HasForeignKey(g => g.SegId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ObservedGradesInstructorCritique>()
                .HasOne<ObservedEvent>()
                .WithMany()
                .HasForeignKey(g => g.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        
        }


    }
}
