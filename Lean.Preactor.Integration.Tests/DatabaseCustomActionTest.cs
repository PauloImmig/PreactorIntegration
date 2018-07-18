using System;
using System.Windows.Forms;
using LSB.App;
using LSB.App.Database;
using LSB.App.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lean.Preactor.Integration.Tests
{
    [TestClass]
    public class DatabaseCustomActionTest
    {
        [TestMethod]
        public void ExecuteStoredProcedure()
        {
            //var testEntity = new DatabaseCustomAction();
            //int timeout = 30;
            //string title = "Titulo teste";
            //string spname = "sp_who";
            //string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=\"AP Ultimate v16.4\";Integrated Security=True";

            //using (AsyncUtil asyncUtil = new AsyncUtil())
            //{
            //    asyncUtil.Start(title);
            //    using (var databaseUtil = new DatabaseUtil(connectionString))
            //    {
            //        try
            //        {
            //            var task = databaseUtil.ExecuteStoredProcedure(spname, timeout);
            //            task.Wait();
            //            MessageBox.Show($"Stored Procedure \"{spname}\" executada com sucesso.");
            //        }
            //        catch
            //        {
            //            MessageBox.Show($"Erro ao executar stored procedure {spname}. Para maiores detalhes consulte o arquivo de logs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}
        }
    }
}
