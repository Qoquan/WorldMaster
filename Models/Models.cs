using System;
using System.Collections.Generic;

namespace WorldMaster.Models;

// Un espace de travail isolé (ex: "Chronique oubliées", "Fading Suns")
// Toutes les autres entités sont rattachées à un UniversId.
public class Univers
{
    public int Id { get; set; }
    public string Nom { get; set; } = "";
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public DateTime DateModification { get; set; } = DateTime.Now;

    public List<Template> Templates { get; set; } = new();
    public List<Fiche> Fiches { get; set; } = new();
    public List<Carte> Cartes { get; set; } = new();
}

// La catégorie générale d'un template ou d'une fiche.
public enum CategorieFiche
{
    Geographie,
    Culture,
    Religion,
    Pnj,
    Monstre,
    Objet,
    Faction,
    Autre
}

// Un template définit la STRUCTURE (les champs) d'un type de fiche.
// Ex: "PNJ important" avec les champs Role, Apparence, Motivation...
public class Template
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public string Nom { get; set; } = "";
    public CategorieFiche Categorie { get; set; }

    // Empêche toute modification/suppression d'un template livré avec l'app.
    // Un template protégé ne peut être QUE dupliqué via "Enregistrer sous".
    public bool EstProtege { get; set; }

    // Si ce template a été créé via "Enregistrer sous" à partir d'un autre.
    public int? TemplateOrigineId { get; set; }

    public List<TemplateChamp> Champs { get; set; } = new();
}

public enum TypeChamp
{
    Texte,
    TexteLong,
    Image,
    Tag,
    Reference,   // référence vers une autre fiche
    Nombre
}

// Un champ défini par un template (ex: "Faiblesse", type TexteLong).
public class TemplateChamp
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public string Nom { get; set; } = "";
    public TypeChamp Type { get; set; }
    public int Ordre { get; set; }
}

// Une fiche est une INSTANCE d'un template (ex: la fiche "Dracula"
// créée à partir du template "PNJ important").
public class Fiche
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public int TemplateId { get; set; }
    public string Nom { get; set; } = "";

    // Les valeurs des champs du template, stockées en JSON.
    // Ex: { "role": "Seigneur vampire", "faiblesse": "Lumière du jour" }
    public string ValeursJson { get; set; } = "{}";

    public DateTime DateModification { get; set; } = DateTime.Now;
}

// Une relation entre deux fiches, utilisée pour l'arbre de liens (mindmap).
// Ex: Dracula --"ennemi de"--> Van Helsing
public class RelationFiche
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public int FicheSourceId { get; set; }
    public int FicheCibleId { get; set; }
    public string TypeRelation { get; set; } = ""; // "allié", "ennemi", "famille"...
}

public enum TypeCarte
{
    Monde,
    Lieu   // taverne, base, donjon...
}

public class Carte
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public string Nom { get; set; } = "";
    public TypeCarte Type { get; set; }

    // Si c'est une carte "Lieu" qui zoome depuis une carte "Monde".
    public int? CarteParenteId { get; set; }

    public string CheminImage { get; set; } = ""; // fichier image de fond
    public List<CartePin> Pins { get; set; } = new();
}

// Un point d'intérêt placé sur une carte, optionnellement lié à une fiche.
public class CartePin
{
    public int Id { get; set; }
    public int CarteId { get; set; }
    public int? FicheId { get; set; } // ex: le pin "Clairval" pointe vers sa fiche ville
    public double X { get; set; }
    public double Y { get; set; }
    public string Label { get; set; } = "";
}

// Le calendrier d'un univers. Un univers peut en avoir plusieurs
// (calendrier civil vs calendrier religieux, par exemple), donc on
// rattache le calendrier à l'univers plutôt que l'inverse.
public class Calendrier
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public string Nom { get; set; } = "Calendrier du monde";

    // true = pré-rempli avec 12 mois de 28-31 jours et 7 jours/semaine,
    // modifiable ensuite comme n'importe quel calendrier personnalisé.
    public bool BaseSurCalendrierDefaut { get; set; } = true;

    public int JoursParSemaine { get; set; } = 7;
    // Stocké en JSON, ex: ["Lundi","Mardi",...] ou des noms inventés.
    public string NomsJoursSemaineJson { get; set; } = "[]";

    // Nom de l'ère utilisé pour l'affichage des années, ex: "après la Chute".
    public string NomEpoque { get; set; } = "";

    // Position actuelle du MJ dans le temps de l'univers.
    public int AnneeActuelle { get; set; } = 1;
    public int? MoisActuelId { get; set; }
    public int JourActuel { get; set; } = 1;

    public List<MoisCalendrier> Mois { get; set; } = new();
}

public class MoisCalendrier
{
    public int Id { get; set; }
    public int CalendrierId { get; set; }
    public string Nom { get; set; } = "";
    public int NombreJours { get; set; }
    public int Ordre { get; set; }
}

// Un événement daté dans le calendrier de l'univers. Peut être lié à
// une fiche du Lore (ex: "Naissance de Dracula") OU à un scénario du
// Tome (ex: date de début du scénario) — un seul des deux à la fois.
public class EvenementCalendrier
{
    public int Id { get; set; }
    public int CalendrierId { get; set; }
    public int? FicheId { get; set; }
    public int? ScenarioId { get; set; }
    public string Titre { get; set; } = "";
    public string Description { get; set; } = "";
    public int Annee { get; set; }
    public int MoisId { get; set; }
    public int Jour { get; set; }
}

// --- Tome (à enrichir plus tard) ---
// Une campagne regroupe plusieurs scénarios.
public class Campagne
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public string Nom { get; set; } = "";
    public string Description { get; set; } = "";
}

public class Scenario
{
    public int Id { get; set; }
    public int UniversId { get; set; }
    public int? CampagneId { get; set; } // un scénario peut exister hors campagne
    public string Nom { get; set; } = "";

    // Contenu riche du scénario, avec mentions @ vers des fiches du Lore.
    public string Contenu { get; set; } = "";

    // Date de début dans le calendrier de l'univers (optionnelle).
    public int? CalendrierId { get; set; }
    public int? AnneeDebut { get; set; }
    public int? MoisDebutId { get; set; }
    public int? JourDebut { get; set; }
}