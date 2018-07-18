using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Dapper;
using LSB.App.Database;
using LSB.App.Database.Model;
using LSB.App.Forms;
using Preactor;
using Preactor.Interop.PreactorObject;

namespace LSB.App
{
    [Guid("4B7822AB-D502-470B-9B18-3DEB1B3A334C")]
    [ComVisible(true)]
    public interface ICalendarCustomAction
    {
        int BreakBars(ref PreactorObj preactorComObject, ref object pespComObject);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("DFA118FF-5963-4E06-A6EB-8BE8BF651894")]
    public class CalendarCustomAction : ICalendarCustomAction
    {
        private DatabaseUtil<QuebraOPQuantidadesPeriodo> _databaseUtil;
        private DatabaseUtil _dbUtil;
        private WaitWindow _loadingWindow;
        private IPreactor _preactor;
        private IPlanningBoard _planningBoard;
        public int BreakBars(ref PreactorObj preactorComObject, ref object pespComObject)
        {
            _preactor = PreactorFactory.CreatePreactorObject(preactorComObject);
            _planningBoard = _preactor.PlanningBoard;

            _loadingWindow = new WaitWindow() { Text = "Executando LSB_APP_Quebra_OP", Visible = true };
            _loadingWindow.Show();

            _dbUtil = new DatabaseUtil(_preactor);
            _dbUtil.OnExecuteStoredProcedureComplete += DbUtil_OnExecuteStoredProcedureComplete;
            _dbUtil.OnExecuteStoredProcedureError += DbUtil_OnExecuteStoredProcedureError;
            _dbUtil.ExecuteStoredProcedure("LSB_APP_Quebra_OP");

            
            return 0;
        }

        private void DbUtil_OnExecuteStoredProcedureError(object sender, string e)
        {
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Close();
                MessageBox.Show($"Erro ao executar stored procedure {e}. Para maiores detalhes consulte o arquivo de logs.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void DbUtil_OnExecuteStoredProcedureComplete(object sender, string e)
        {
            _databaseUtil = new DatabaseUtil<QuebraOPQuantidadesPeriodo>(_preactor);
            _databaseUtil.OnExecuteQueryComplete += _databaseUtil_OnExecuteQueryComplete;
            _databaseUtil.OnExecuteQueryError += _databaseUtil_OnExecuteQueryError;

            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Text = "Executando quebra de barras";
            }));
            _databaseUtil.ExecuteQuery(QuebraOPQuantidadesPeriodo.QUERY);
        }

        private void _databaseUtil_OnExecuteQueryError(object sender, string e)
        {
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Close();
                MessageBox.Show(e, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }));
        }

        private void _databaseUtil_OnExecuteQueryComplete(object sender, System.Collections.Generic.IEnumerable<QuebraOPQuantidadesPeriodo> calendarState)
        {
            int count = 0;
            var databaseUtil = new DatabaseUtil<QuebraOPQuantidadesPeriodo>(_preactor);
            foreach (var item in calendarState.OrderBy(x => x.Id))
            {
                try
                {
                    var resourceNumber = _planningBoard.GetResourceNumber(item.Recurso.ToString());
                    var quantity = _planningBoard.GetProcessedQuantity(resourceNumber, item.Operacao, item.Inicio, item.Fim);
                    count++;
                    item.Quantidade = quantity;
                    databaseUtil.Save(item);
                }
                catch (Exception ex)
                {

                }
            }
            _loadingWindow.Invoke(new Action(() =>
            {
                _loadingWindow.Close();
            }));
        }
    }
}
