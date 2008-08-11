using System.Collections.Generic;

namespace ExecuteTemplate.Tests
{
	//[TestFixture]
	//public class FormatWhereBlockTest
	//{
	//    [Test]
	//    public void SingleInt()
	//    {
	//        List<int> i = new List<int>();
	//        i.Add(1);
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test = 1) ");
	//    }
		
	//    [Test]
	//    public void MultiInt()
	//    {
	//        List<int> i = new List<int>();
	//        i.Add(1);
	//        i.Add(2);
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test = 1 or test = 2) ");
	//    }
		
	//    [Test]
	//    public void MetaSymbol()
	//    {
	//        List<string> i = new List<string>();
	//        i.Add("123*123");
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test like '123%123') ");
	//    }

	//    [Test]
	//    public void MultiMetaSymbol()
	//    {
	//        List<string> i = new List<string>();
	//        i.Add("123*123");
	//        i.Add("sd*123");
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test like '123%123' or test like 'sd%123') ");
	//    }
		
	//    [Test]
	//    public void MetaSumbolAndStringLikeInt()
	//    {
	//        List<string> i = new List<string>();
	//        i.Add("123*123");
	//        i.Add("123");
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test like '123%123' or test = 123) ");
	//    }
		
	//    [Test]
	//    public void Decimal()
	//    {
	//        List<decimal> i = new List<decimal>();
	//        i.Add((decimal) 123.5);
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test = 123,5) ");			
			
	//    }

	//    [Test]
	//    public void DecimalInString()
	//    {
	//        List<string> i = new List<string>();
	//        i.Add("123,5");
	//        Assert.IsTrue(QueryBuilderUtils.FormatWhereBlock(i, "test") == " (test = 123,5) ");			
	//    }
	//}
}
