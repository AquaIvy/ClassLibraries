using Aquaivy.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace TestCore
{
    [TestClass]
    public class TestCharacterUtils
    {
        public TestContext TestContext { get; set; }

        [DataTestMethod]
        [DataRow('��', 3)]
        public void Test_GetUTF8Length(char c, int len)
        {
            //int length = CharacterUtils.GetUTF8Length(c);
            //Assert.IsTrue(length == len);

            //string str = "�����ҵ���һ���޽����������ǰ���崵�ɽ�ң���������Զֻ������ˮһ̶ ų������ֻ����㲻ǰ��çײ����ֻ����Ϊ����ֻ�������¸ҵ��˲�����������";
            //for (int i = 0; i < str.Length; i++)
            //{
            //    char _c = str[i];

            //    TestContext.WriteLine($"{_c}  {CharacterUtils.GetUTF8CharBytesLength(_c)}");
            //}

            string s = "�����ҵ���һ����";

            TestContext.WriteLine($"{s}  {EncodingUtils.GetEncodingBytesLength(s, Encoding.BigEndianUnicode)}");
        }


        [DataTestMethod]
        [DataRow("123,_!@#", "\\u0031\\u0032\\u0033\\u002C\\u005F\\u0021\\u0040\\u0023")]
        [DataRow("����", "\\u6C49\\u5B57")]
        [DataRow("���", "\\u5982\\u679C")]
        [DataRow("����", "\\u751F\\u547D")]
        public void Test_GetUnicode(string str, string unicode)
        {
            TestContext.WriteLine($"{str}  {EncodingUtils.GetUnicode(str)}");
        }

        [DataTestMethod]
        [DataRow("\\u6C49\\u5B57", "����")]
        [DataRow("\\u5982\\u679C", "���")]
        public void Test_UnicodeToString(string unicode, string str)
        {
            TestContext.WriteLine($"{unicode}  {EncodingUtils.UnicodeToString(unicode)}");
        }
    }
}
