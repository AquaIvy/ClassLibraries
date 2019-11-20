using Aquaivy.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPinyin;

namespace TestCore
{
    [TestClass]
    public class TestNPinyin
    {
        [DataTestMethod]
        [DataRow("��", "W")]
        [DataRow("¯��ʯϴ��", "LGSXJ")]
        [DataRow("��ù�����", "HMSRG")]
        public void Test_Initials(string hanzi, string pinyin)
        {
            string converted = Pinyin.GetInitials(hanzi);
            Assert.IsTrue(converted == pinyin);
        }

        [DataTestMethod]
        [DataRow("��", "wo")]
        [DataRow("�³�����Υ����������Ϊ", "shi chang yu ren wei �� shi zong zai ren wei")]
        [DataRow("�������ܳ����ģ�ǿ���Ǵ������", "jun ma shi pao chu lai de �� qiang bing shi da chu lai de")]
        public void Test_GetPinyin(string hanzi, string pinyin)
        {
            string converted = Pinyin.GetPinyin(hanzi);
            Assert.IsTrue(converted == pinyin);
        }

        [DataTestMethod]
        [DataRow("ai", "�������������������������������������������")]
        [DataRow("shi", "��ʱʮʹ��ʵʽʶ����ʯʲʾ��ʷʦʼʩʿ��ʪ��ʳʧ������ʴʫ��ʰ��ʻʨʬʭʸʺ����������������������ݪ��߱���������������������")]
        public void Test_GetChineseText(string pinyin, string hanzi)
        {
            string converted = Pinyin.GetChineseText(pinyin);
            Assert.IsTrue(converted == hanzi);
        }
    }
}
