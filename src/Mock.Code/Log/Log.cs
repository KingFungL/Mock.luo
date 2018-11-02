
using log4net;

namespace Mock.Code.Log
{
    public class Log
    {
        private ILog _logger;
        public Log(ILog log)
        {
            this._logger = log;
        }
        public void Debug(object message)
        {
            this._logger.Debug(message);
        }
        public void Error(object message)
        {
            this._logger.Error(message);
        }
        public void Info(object message)
        {
            this._logger.Info(message);
        }
        public void Warn(object message)
        {
            this._logger.Warn(message);
        }
    }
}
