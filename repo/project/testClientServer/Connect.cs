
using System.Data;
using System.Data.SqlClient;
using TransmittedPackets;
namespace testBD
{
    class DBManager
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private bool isConnection = false;
        
        /// <summary>
        /// Подключение к базе данных.
        /// </summary>
        public void Connect()
        {
            this.connection = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=Chess;Integrated Security=True");
            this.connection.Open();
            this.RechangeConnectionStatus();
        }

        /// <summary>
        /// Добавляет в базу обьект типа SaveHystoryParty
        /// </summary>
        /// <param name="hystoryParty"></param>
        public void Added(SaveHystoryParty hystoryParty)
        {
            if (isConnection)
            {
                MementoStep step;
                Analitic analictic = new Analitic();
                foreach (var itemHystoryList in hystoryParty.Hystory)
                {                    
                    step = itemHystoryList;

                    if (analictic.ValidationArrangementFigures(step))
                    {
                        this.command = new SqlCommand("INSERT INTO StorageBestMoves(ArrangementFigures, Direction, Position, DepthOfGame)" +
                                                      "VALUES (@ArrangementFigures, @Direction, @Position, @DepthOfGame);", connection);
                        this.command.Parameters.Add("@ArrangementFigures", System.Data.SqlDbType.VarBinary);
                        this.command.Parameters["@ArrangementFigures"].Value = SerealizationManager.Serealize(step.ArrangementFigures);
                        this.command.Parameters.Add("@Direction", System.Data.SqlDbType.VarBinary);
                        this.command.Parameters["@Direction"].Value = SerealizationManager.Serealize(step.direction);
                        this.command.Parameters.Add("@Position", System.Data.SqlDbType.VarBinary);
                        this.command.Parameters["@Position"].Value = SerealizationManager.Serealize(step.position);
                        this.command.Parameters.Add("@DepthOfGame", System.Data.SqlDbType.Int);
                        this.command.Parameters["@DepthOfGame"].Value = step.stepIndex;
                        
                        this.command.ExecuteNonQuery();
                    }
                    else
                        throw new System.ArgumentException("Index одной или нескльких фигур не соответствую заданой конигурации");
                }
            }
            else
                throw new System.ArgumentException("Нет подключения к базе данных");
        }

        private DataTable GetData()
        {
            if (isConnection)
            {
                DataTable table = new DataTable();
                this.command = new SqlCommand("SELECT * FROM StorageBestMoves", connection);
                this.adapter = new SqlDataAdapter(this.command);
                this.adapter.Fill(table);
                return table;
            }
            else
                throw new System.ArgumentException("Нет подключения к базе данных");
        }
        int sdf = 0;

         
        private DataTable GetData(int DepthOfGame)
        {
            if (isConnection)
            {
                DataTable table = GetData();
                int MIN = DepthOfGame - 3;
                int MAX = DepthOfGame + 3;

                if (MIN < 0)
                    MIN = 0;
                if (MAX > table.Rows.Count - 1)
                    MAX = table.Rows.Count - 1; 
               
                this.command = new SqlCommand("SELECT * FROM StorageBestMoves where DepthOfGame BETWEEN @MIN AND @MAX", connection);
                this.command.Parameters.Add("@MIN", System.Data.SqlDbType.Int);
                this.command.Parameters["@MIN"].Value = MIN;
                this.command.Parameters.Add("@MAX", System.Data.SqlDbType.Int);
                this.command.Parameters["@MAX"].Value = MAX;
                this.adapter = new SqlDataAdapter(this.command);

                 
                this.adapter.Fill(table);
                return table;
            }
            else
                throw new System.ArgumentException("Нет подключения к базе данных");
        }
        private DataTable GetData(string SqlComand)
        {
            if (isConnection)
            {
                DataTable table = new DataTable();
                this.command = new SqlCommand("SqlComand", connection);
                this.adapter = new SqlDataAdapter(this.command);
                this.adapter.Fill(table);
                return table;
            }
            else
                throw new System.ArgumentException("Нет подключения к базе данных");
        }

        private void RechangeConnectionStatus()
        {
            isConnection = !isConnection;
            
            
        }
        /// <summary>
        /// Возвращает обьект типа ChosenStep 
        /// </summary>
        /// <returns></returns>
        public ChosenStep GetChosenStep(RequestHalfStep requestHalfStep)
        {
            DataTable table = GetData(10);
            Analitic analitic = new Analitic();
            return analitic.FindTheBestStep(requestHalfStep, table);
        }

     public   void Close()
        {
            if (isConnection)
            {
                this.connection.Close();
                this.RechangeConnectionStatus();
                isConnection = false;
            }
        }

        //public void Delete(int idRow) { }

        //public void StartCleaning(int idRow) { }

       
    }
    class Analitic
    { 


        public int AverageDepthGame { get; private set; }
        public int ClaclAverageDepthGame(DataTable data)
        {
            int buffer = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                buffer += (int)data.Rows[i]["DepthOfGame"];
            }
            buffer /= data.Rows.Count;
            return buffer;
        }

        /// <summary>
        /// Проверяет индекс фигур в матрице состояние игровой доски на текущий шаг.
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public bool ValidationArrangementFigures(MementoStep step) // Доработать!
        {
            //===============================================================================
            // Список ид-шников фигур. = > 1-6 - ид фигуры, 7-12 - тегированый ид фигуры (+1)
            // 13 - 0, пустая клетка. 
            // Доработать ( а) Конфиг файл, б) добавить ид в dll )   
            byte[] idFigures = new byte[13];
            for (int i = 0; i < idFigures.Length-1; i++)
            {
                idFigures[i] = (byte)i;
            }
            idFigures[12] = 0; 
            //===============================================================================

            bool flag = false;
            for (int i = 0; i < step.ArrangementFigures.GetLength(0); i++)
            {
                for (int j = 0; j < step.ArrangementFigures.GetLength(1); j++)
                {
                    foreach (var item in idFigures) // < =====================================
                    {
                        // если елементы совпадают выходим из цыкла и проверяем следующий елемент.
                        if (step.ArrangementFigures[i, j] == item)
                        {
                            flag = true;
                            break;
                        }
                        else  // если не совпадают сравниваем дальше
                            flag = false;
                    }
                    if (flag == false) // если текущий елемент не подходит условиям возвращает фальш.
                        return flag;
                }
            }
            return flag;
        }

        public ChosenStep FindTheBestStep(RequestHalfStep requestHalfStep, DataTable table)
        {
            // if (ValidationArrangementFigures(requestHalfStep.)) {
            byte[,] tmp_ArrangementFigures;
            System.Collections.Generic.List<MementoItemAssessment> ListItemAssessment =
                new System.Collections.Generic.List<MementoItemAssessment>();
            float oneProcent = 100 / 64; 
            float procent = 0;
            for (int indexRow = 0; indexRow < table.Rows.Count; indexRow++)
            {
                byte[] array = (byte[])table.Rows[indexRow]["ArrangementFigures"];          
                tmp_ArrangementFigures = (byte[,])SerealizationManager.Deserealize(array);

                for (int i = 0; i < tmp_ArrangementFigures.GetLength(0); i++)
                    for (int j = 0; j < tmp_ArrangementFigures.GetLength(1); j++)
                        if (tmp_ArrangementFigures[i, j] == requestHalfStep.ArrangementFigures[i, j])
                            procent += oneProcent;
                ListItemAssessment.Add(new MementoItemAssessment(indexRow, procent));
            }

            ListItemAssessment.Sort((a, b) => a.procent.CompareTo(b.procent));
            ListItemAssessment.Reverse(); 
            foreach (var item in ListItemAssessment)
            {
                byte[] array = (byte[])table.Rows[item.indexRow]["ArrangementFigures"];
                tmp_ArrangementFigures = (byte[,])SerealizationManager.Deserealize(array);
                array = (byte[])table.Rows[item.indexRow]["Position"];
                PointV2 pos = (PointV2)SerealizationManager.Deserealize(array);
                array = (byte[])table.Rows[item.indexRow]["Direction"];
                PointV2 dir = (PointV2)SerealizationManager.Deserealize(array);
                if (requestHalfStep.ArrangementFigures[pos.x, pos.y] == tmp_ArrangementFigures[pos.x, pos.y])
                    return new ChosenStep(pos, dir);
            }
             throw new System.ArgumentException("На данный момент в базе нет подходящих значений");
        }
            

        /// <summary>
        /// Хранит отценку 
        /// </summary>
      private class MementoItemAssessment
        {
            public float procent { get; private set; }
            public int indexRow { get; private set; }

            public MementoItemAssessment(int indexRow, float procent)
            {
                this.procent = procent;
                this.indexRow = indexRow;
            }
        }
    }
}
