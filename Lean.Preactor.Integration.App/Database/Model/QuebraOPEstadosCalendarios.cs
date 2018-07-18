using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSB.App.Database.Model
{
    public class QuebraOPEstadosCalendarios
    {
        //        public const string QUERY = @"
        //SELECT [datasetid] as DatasetId
        //      ,[Id_recurso] as IdRecurso
        //      ,[recurso] as Recurso
        //      ,[Id_calendario] as IdCalendario
        //      ,[calendario] as Calendario
        //      ,[estado] as Estado
        //      ,[data_referencia] as DataReferencia
        //      ,[inicio_estado] as InicioEstado
        //      ,[fim_estado] as FimEstado
        //  FROM[dbo].[LSB_Quebra_OP_Estados_Calendarios]";
        public const string QUERY = @"
SELECT [datasetid] as datasetid
      ,[Id_recurso] as IdRecurso
      ,[recurso] as Recurso
      ,[Id_calendario] as IdCalendario
      ,[calendario] as Calendario
      ,[estado] as Estado
      ,[data_referencia] as DataReferencia
      ,[inicio_estado] as InicioEstado
      ,[fim_estado] as FimEstado
  FROM[dbo].[LSB_APP_Quebra_OP_Estados_Calendarios]";
        public int DatasetId { get; set; }
        public int IdRecurso { get; set; }
        public string Recurso { get; set; }
        public int IdCalendario { get; set; }
        public string Calendario { get; set; }
        public string Estado { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime InicioEstado { get; set; }
        public DateTime FimEstado { get; set; }
    }
}
