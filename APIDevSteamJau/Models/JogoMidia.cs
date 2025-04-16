﻿namespace APIDevSteamJau.Models
{
    public class JogoMidia
    {
        public Guid JogoMidiaId { get; set; }
        public Guid JogoId { get; set; }
        public Jogo? Jogo { get; set; }
        public string Tipo { get; set; } // Ex: Trailer, Imagem
        public string Url { get; set; } // URL do trailer ou imagem
    }
}
