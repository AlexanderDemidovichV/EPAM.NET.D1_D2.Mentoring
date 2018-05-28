using Interop.MSUtil;

namespace LoggingReport
{
    public class LoggingReportService
    {
        private static readonly COMTSVInputContextClass inputContext = new COMTSVInputContextClass 
        {
            headerRow = false,
            iSeparator = "|",
            nFields = 3
        };

        private readonly string path;

        public LoggingReportService(string path)
        {
            this.path = path;
        }

        public ILogRecordset Info => new LogQueryClass().Execute(
            $@"SELECT Field1 AS Date, Field2 AS Level, Field3 AS Message FROM {path} WHERE Field2='INFO'", inputContext);
        
        public ILogRecordset Error => new LogQueryClass().Execute(
            $@"SELECT Field1 AS Date, Field2 AS Level, Field3 AS Message FROM {path} WHERE Field2='ERROR'", inputContext);

        public ILogRecordset Debug => new LogQueryClass().Execute(
            $@"SELECT Field1 AS Date, Field2 AS Level, Field3 AS Message FROM {path} WHERE Field2='DEBUG'", inputContext);

        public ILogRecordset LevelsCount => new LogQueryClass().Execute(
            $@"SELECT Field2 AS Level, COUNT(*) AS LevelsCount FROM {path} GROUP BY Field2", inputContext);
    }
}
