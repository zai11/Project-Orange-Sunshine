using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ProjectOrangeSunshine.Shared
{
    public class Connection
    {

        private bool success;
        private int errorCode;
        private string errorMessage = "";
        private NetworkStream? stream;

        public bool Success { get { return success; } set { success = value; } }

        public int ErrorCode { get { return errorCode; } set { errorCode = value; } }

        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }

        public NetworkStream? Stream { get { return stream; } set { stream = value; } }

        public Connection(bool success, NetworkStream stream)
        {
            this.success = success;
            this.stream = stream;
        }

        public Connection(bool success, NetworkStream? stream, int errorCode, string errorMessage)
        {
            this.success = success;
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.stream = stream;
        }
    }
}
