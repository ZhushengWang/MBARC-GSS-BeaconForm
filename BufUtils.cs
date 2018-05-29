using System;
using System.Text;
using System.IO;


namespace BufUtilsLib
{
    public static class BufUtils
    {
        public static byte ms_byte(int val)
        {
            return (byte)((val & 0xFF000000) >> 24);
        }

        public static byte ls2_byte(int val)
        {
            return (byte)((val & 0x00FF0000) >> 16);
        }

        public static byte ls1_byte(int val)
        {
            return (byte)((val & 0x0000FF00) >> 8);
        }

        public static byte ls_byte(int val)
        {
            return (byte)(val & 0x000000FF);
        }

        // Get a 32-bit that is stored in a byte array
        // in big endian order
        public static int Get32BitInt(ref byte[] buf, int index)
        {
            int ms_byte = ((int)(buf[index])) << 24;
            int ls2_byte = ((int)(buf[index + 1])) << 16;
            int ls1_byte = ((int)(buf[index + 2])) << 8;
            int ls_byte = (int)(buf[index + 3]);
            return ms_byte | ls2_byte | ls1_byte | ls_byte;
        }

        // Put a 32-bit int in a byte array in big endian order
        public static void Put32BitInt(ref byte[] buf, int index, int new_value)
        {
            buf[index] = ms_byte(new_value);
            buf[index + 1] = ls2_byte(new_value);
            buf[index + 2] = ls1_byte(new_value);
            buf[index + 3] = ls_byte(new_value);
        }

        // Get a 32-bit that is stored in a byte array
        // in big endian order
        public static int Get16BitInt(ref byte[] buf, int index)
        {
            int ms_byte = ((int)(buf[index])) << 8;
            int ls_byte = (int)(buf[index + 1]);
            return ms_byte | ls_byte;
        }

        // Put a 16-bit int in a byte array in big endian order
        public static void Put16BitInt(ref byte[] buf, int index, int new_value)
        {
            buf[index] = ls1_byte(new_value);
            buf[index + 1] = ls_byte(new_value);
        }


        // Return a string containing a formatted hex dump of buf
        //  starting at buf_index.
        public static String HexDumpString(ref byte[] buf, int buf_index, int len)
        {
            const int bytesPerLine = 16;
            String hexDumpStr = "";
            int i = buf_index;
            int end_index = buf_index + len;
            String line;
            int line_bytes;

            while (i < end_index)
            {
                line = "0x" + i.ToString("X4") + " (" + i.ToString("D4") + "):  ";
                line_bytes = 0;
                while (line_bytes++ < bytesPerLine)
                {
                    line += buf[i++].ToString("X2") + " ";
                    if (i == end_index) { break; }
                }
                hexDumpStr += line + Environment.NewLine;
            }
            return hexDumpStr;
        }

        // Return a string containing a formatted hex dump of buf
        //  starting at buf_index.
        public static String HexDumpString(ref byte[] buf,
                                            int buf_index,
                                            int len,
                                            String line_prefix,
                                            int displayed_index_offset)
        {
            const int bytesPerLine = 16;
            String hexDumpStr = "";
            int i = buf_index;
            int end_index = buf_index + len;
            String line;
            int line_bytes;

            while (i < end_index)
            {
                line = "0x" + (i + displayed_index_offset).ToString("X4")
                                + " (" + (i + displayed_index_offset).ToString("D4") + "):  ";
                line_bytes = 0;
                while (line_bytes++ < bytesPerLine)
                {
                    line += buf[i++].ToString("X2") + " ";
                    if (i == end_index) { break; }
                }
                hexDumpStr += line_prefix + line + Environment.NewLine;
            }
            return hexDumpStr;
        }


    }
}
