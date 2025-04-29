﻿using APIDevSteamJau.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIDevSteamJau.Data
{
    public class APIContext : IdentityDbContext<Usuario>
    {
        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {
        }

        // DbSet
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<JogoMidia> JogoMidia { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<JogoCategoria> JogosCategorias { get; set; }

        public DbSet<Carrinho> Carrinhos { get; set; }
        public DbSet<ItemCarrinho> ItemCarrinhos { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Tabelas
            builder.Entity<Jogo>().ToTable("Jogos");
            builder.Entity<JogoMidia>().ToTable("JogosMidia");
            builder.Entity<Categoria>().ToTable("Categorias");
            builder.Entity<JogoCategoria>().ToTable("JogosCategorias");
            builder.Entity<Carrinho>().ToTable("Carrinhos");
            builder.Entity<ItemCarrinho>().ToTable("ItensCarrinho");

        }
    }
}