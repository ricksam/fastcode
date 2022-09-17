using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Repositories.Models
{
    public class LogApi
    {
        public int Id { get; set; }
        public DateTime Cadastro { get; set; }
        public decimal Milissegundos { get; set; }
        public string Ip { get; set; }
        public string Metodo { get; set; }
        public string Url { get; set; }
        public string Autorizacao { get; set; }
        public string Requisicao { get; set; }
        public string Resposta { get; set; }
        public bool Erro { get; set; }
    }
}
