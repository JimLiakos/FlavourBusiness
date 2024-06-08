using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PrinterEmulator
{
    public class PrinterTCPListener
    {
        public PrinterTCPListener(Printer printer) {

            Printer = printer;
        }

        public Printer Printer { get; }


        public void RunAsync()
        {

            Task.Run(() =>
            {
                {
                    TcpListener server = null;
                    try
                    {
                        // Set the TcpListener on port 13000.
                        Int32 port = Printer.Port;
                        IPAddress localAddr = IPAddress.Any;

                        // TcpListener server = new TcpListener(port);
                        server = new TcpListener(localAddr, port);

                        // Start listening for client requests.
                        server.Start();

                        // Buffer for reading data
                        Byte[] bytes = new Byte[256];
                        String data = null;

                        // Enter the listening loop.
                        while (true)
                        {
                         

                            // Perform a blocking call to accept requests.
                            // You could also use server.AcceptSocket() here.
                            using (TcpClient client = server.AcceptTcpClient())

                            {
                         

                                data = null;

                                // Get a stream object for reading and writing
                                NetworkStream stream = client.GetStream();

                                int i;

                                // Loop to receive all the data sent by the client.
                                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    // Translate data bytes to a ASCII string.
                                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                    Console.WriteLine("Received: {0}", data);

                                    // Process the data sent by the client.
                                    data = data.ToUpper();

                                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);



                                    if (i == 3 && bytes[0] == 0x10 && bytes[1] == 0x4 && bytes[2] == 0x1)//get printer status
                                    {
                                        stream.Write(Printer.Status.Take(1).ToArray(), 0, 1);
                                        stream.Flush();
                                    }
                         
                                }
                            }
                        }
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("SocketException: {0}", e);
                    }
                    finally
                    {
                        server.Stop();
                    }

                    Console.WriteLine("\nHit enter to continue...");
                    Console.Read();
                }
            });
        }
    }
}
