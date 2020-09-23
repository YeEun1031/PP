namespace FUP
{
    public class CONSTANTS
    {
        public const uint REQ_FILE_SEND = 0x01;
        public const uint REP_FILE_SEND = 0x02;
        public const uint FILE_SEND_DATA = 0x03;
        public const uint FILE_SEND_RES = 0x04;

        public const byte NOT_FRAGMENTED = 0x00;
        public const byte FRAGMENT = 0x01;

        public const byte NOT_LASTMSG = 0x00;
        public const byte LASTMSG = 0x01;

        public const byte ACCEPIED = 0x00;
        public const byte DENIED = 0x01;

        public const byte FAIL = 0x00;
        public const byte SUCCESS = 0x01;
    }

    // 메시지, 헤더, 바디가 모두 상속하는 인터페이스
    public interface ISerializable
    {
        byte[] GetBytes();
        int GetSize();
    }

    // FUP의 메시지를 나타내는 클래스 헤더와 바디로 구성
    public class Message : ISerializable
    {
        public Header Header { get; set; }
        public ISerializable Body { get; set; }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[GetSize()];

            Header.GetBytes().CopyTo(bytes, 0);
            Body.GetBytes().CopyTo(bytes, Header.GetSize());

            return bytes;
        }

        public int GetSize()
        {
            return Header.GetSize() + Body.GetSize();
        }
    }
}
