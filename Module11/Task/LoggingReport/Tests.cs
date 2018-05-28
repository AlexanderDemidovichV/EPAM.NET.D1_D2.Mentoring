using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace LoggingReport
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void GenerateLoggingReport()
        {
            var report = new StringBuilder();

            var resultDataSet = new LoggingReportService("d:\\Log.txt").LevelsCount;

            while (!resultDataSet.atEnd()) {
                var record = resultDataSet.getRecord();
                report.AppendFormat("{0} | {1}{2}", record.getValue("Level"), record.getValue("LevelsCount"), Environment.NewLine);
                resultDataSet.moveNext();
            }

            resultDataSet = new LoggingReportService("d:\\Log.txt").Error;
            while (!resultDataSet.atEnd()) {
                var record = resultDataSet.getRecord();
                report.AppendFormat("{0} | {1} | {2}{3}", record.getValue("Date"), record.getValue("Level"), record.getValue("Message"), Environment.NewLine);
                resultDataSet.moveNext();
            }

            File.WriteAllText(@"D:\Report.txt", report.ToString());
        }
    }
}
