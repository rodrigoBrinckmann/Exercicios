using System.Text.Json.Serialization;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class MovimentacaoConta
    {               
        public string ContaCorrente { get; set; }
        public EnumTipoMovimento TipoMovimento { get; set; }
        public decimal Valor { get; set; }
        [JsonIgnore]
        public string? IdContaCorrente { get; set;}
    }
}
