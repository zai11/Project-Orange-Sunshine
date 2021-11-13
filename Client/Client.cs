using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Client
    {
        public TcpClient socket;
        public NetworkStream stream;

        public ByteBuffer buffer;
        private byte[] receiveBuffer;

    }
}
