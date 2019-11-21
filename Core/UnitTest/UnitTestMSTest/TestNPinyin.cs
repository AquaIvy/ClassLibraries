using Aquaivy.Core.Utilities;
using Aquaivy.Pinyin.PinyinMap;
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

        [TestMethod]
        public void Test_LoadMapFile()
        {
            PinyinMapController map = new PinyinMapController();
            map.LoadMapFile(@"D:\Unity\VSProjects\ClassLibraries\Core\UnitTest\UnitTestMSTest\PinyinMapDest.txt");

            var mapValue = map.Query("drq");
            var mapValue1 = map.Query("kg");
            var mapValue2 = map.Query("mm");
        }

        [TestMethod]
        public void Test_PinyinMapCreater()
        {
            var srcPath = @"..\..\..\PinyinMapSource.txt";
            var destPath = @"..\..\..\PinyinMapDest.txt";
            PinyinMapCreater.Create(srcPath, destPath);
        }
    }
}
