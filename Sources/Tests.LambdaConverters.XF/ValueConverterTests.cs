using System.Globalization;
using LambdaConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.LambdaConverters.XF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WithConvertFunction()
        {
            // with a wrong target type (use default error strategy)
            Assert.IsNull(ValueConverter.Create<int, string?>(e => null).Convert(1, typeof(bool), null, null));

            // without a target type
            Assert.AreEqual("a", ValueConverter.Create<int, string>(e => "a").Convert(1, null, null, null));

            // with an unexpected parameter (use default error strategy)
            Assert.IsNull(ValueConverter.Create<int, string?>(e => null).Convert(1, typeof(string), "p", null));

            // with an input value of an unexpected type (use default error strategy)
            Assert.IsNull(ValueConverter.Create<int, string?>(e => null).Convert(true, typeof(string), null, null));
            Assert.IsNull(ValueConverter.Create<int, string?>(e => null).Convert(null, typeof(string), null, null));

            // with a valid input value
            Assert.AreEqual("a", ValueConverter.Create<int, string>(e => "a").Convert(1, typeof(string), null, null));
            Assert.AreEqual("1", ValueConverter.Create<int, string>(
                e =>
                {
                    Assert.AreEqual(1, e.Value);
                    Assert.IsNull(e.Culture);

                    return e.Value.ToString();
                }).Convert(1, typeof(string), null, null));
            Assert.AreEqual(
                "1",
                ValueConverter.Create<int, string>(
                    e =>
                    {
                        Assert.AreEqual(1, e.Value);
                        Assert.AreEqual(new CultureInfo("en-GB"), e.Culture);

                        return e.Value.ToString();
                    }).Convert(1, typeof(string), null, new CultureInfo("en-GB")));
        }
    }
}
