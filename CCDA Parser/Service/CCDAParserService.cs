using CCDA_Parser.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CCDA_Parser.Service
{
    public class CCDAParserService
    {
        private readonly System.Timers.Timer _timer;

        public CCDAParserService()
        {
            _timer = new System.Timers.Timer(5000000);
            _timer.Elapsed += TimeElapsed;
        }

        private async void TimeElapsed(object? sender, ElapsedEventArgs e)
        {
            string FileName = @"D:/Project/IDoc/IDOC(Documents)/Sample.xml";
            CCDAParserDAL parserDAL = new CCDAParserDAL();
            //await parserDAL.XmlDataParhing(FileName);
        }

        public void start()
        {
            _timer.Start();
        }

        public void stop()
        {
            _timer.Stop();
        }
    }
}
