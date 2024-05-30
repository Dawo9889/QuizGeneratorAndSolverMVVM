using Microsoft.EntityFrameworkCore;
using PierwszePodejscieDoQuizu.Database.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.Database
{
    public class QuizDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Pobierz bieżący katalog roboczy (np. bin\Debug\net8.0-windows)
            string currentDirectory = Directory.GetCurrentDirectory();

            // Przejdź o trzy poziomy w górę, aby dojść do katalogu głównego projektu
            string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;

            // Utwórz pełną ścieżkę do pliku bazy danych w katalogu głównym projektu
            string databasePath = Path.Combine(projectDirectory, "Quizzes.sqlite");

            // Skonfiguruj SQLite z nową ścieżką
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }

}
