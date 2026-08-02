using Microsoft.EntityFrameworkCore;
using WorldMaster.Models;

namespace WorldMaster.Data;

public class AppDbContext : DbContext
{
    // Le fichier .db sera créé dans le dossier de données de l'app,
    // ce qui évite les soucis de permissions une fois l'app installée.
    public static string DbPath =>
        Path.Combine(FileSystem.AppDataDirectory, "worldmaster.db");

    public DbSet<Univers> Univers => Set<Univers>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<TemplateChamp> TemplateChamps => Set<TemplateChamp>();
    public DbSet<Fiche> Fiches => Set<Fiche>();
    public DbSet<RelationFiche> RelationsFiches => Set<RelationFiche>();
    public DbSet<Carte> Cartes => Set<Carte>();
    public DbSet<CartePin> CartePins => Set<CartePin>();
    public DbSet<Calendrier> Calendriers => Set<Calendrier>();
    public DbSet<MoisCalendrier> MoisCalendriers => Set<MoisCalendrier>();
    public DbSet<EvenementCalendrier> EvenementsCalendrier => Set<EvenementCalendrier>();
    public DbSet<Campagne> Campagnes => Set<Campagne>();
    public DbSet<Scenario> Scenarios => Set<Scenario>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Un univers supprimé entraîne la suppression de tout son contenu.
        modelBuilder.Entity<Template>()
            .HasOne<Univers>()
            .WithMany(u => u.Templates)
            .HasForeignKey(t => t.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Fiche>()
            .HasOne<Univers>()
            .WithMany(u => u.Fiches)
            .HasForeignKey(f => f.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TemplateChamp>()
            .HasOne<Template>()
            .WithMany(t => t.Champs)
            .HasForeignKey(c => c.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Carte>()
            .HasOne<Univers>()
            .WithMany(u => u.Cartes)
            .HasForeignKey(c => c.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartePin>()
            .HasOne<Carte>()
            .WithMany(c => c.Pins)
            .HasForeignKey(p => p.CarteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Calendrier>()
            .HasOne<Univers>()
            .WithMany()
            .HasForeignKey(c => c.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MoisCalendrier>()
            .HasOne<Calendrier>()
            .WithMany(c => c.Mois)
            .HasForeignKey(m => m.CalendrierId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EvenementCalendrier>()
            .HasOne<Calendrier>()
            .WithMany()
            .HasForeignKey(e => e.CalendrierId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Campagne>()
            .HasOne<Univers>()
            .WithMany()
            .HasForeignKey(c => c.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Scenario>()
            .HasOne<Univers>()
            .WithMany()
            .HasForeignKey(s => s.UniversId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Scenario>()
            .HasOne<Campagne>()
            .WithMany()
            .HasForeignKey(s => s.CampagneId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}